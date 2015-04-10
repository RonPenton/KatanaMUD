using System;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Framework.DependencyInjection;
using System.Net.WebSockets;
using System.Threading.Tasks;
using System.Threading;
using System.Text;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.AspNet.Hosting;
using KatanaMUD.Models;
using Microsoft.AspNet.Security.Cookies;
using Microsoft.AspNet.StaticFiles;
using KatanaMUD.Messages;
using System.Linq;

namespace KatanaMUD
{
	public class Startup
	{
		public IConfiguration Configuration { get; set; }

		public Startup(IHostingEnvironment env)
		{
			// Setup configuration sources.
			//Configuration = new Configuration()
			//	.AddJsonFile("config.json")
			//	.AddEnvironmentVariables();
		}

		// For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDataProtection();
			services.AddAuthorization();
			services.AddMvc();
		}

		public void Configure(IApplicationBuilder app)
		{
			var thread = new Thread(Game.Run);
			thread.Start();

            app.UseErrorPage();

			app.UseCookieAuthentication(opts =>
			{
				opts.AuthenticationType = CookieAuthenticationDefaults.AuthenticationType;
				opts.LoginPath = new PathString("/Account/Login");
			});

			app.Use(async (HttpContext context, Func<Task> next) =>
			{
				if (context.IsWebSocketRequest)
				{
					var socket = await context.AcceptWebSocketAsync(context.WebSocketRequestedProtocols[0]);
                    //Console.WriteLine("Incoming connection: " + context.Request.Host.Value);

                    if (!context.User?.Identity.IsAuthenticated ?? true)
                    {
                        //Console.WriteLine("Connection Aborted: Not Authorized.");
                        var rejection = new LoginRejected() { RejectionMessage = "User is not authenticated" };
                        Connection.SendMessage(socket, rejection);
                        await socket.CloseAsync(WebSocketCloseStatus.PolicyViolation, "User is not authenticated", CancellationToken.None);
                        return;
                    }

                    var user = Game.Data.Users.SingleOrDefault(x => x.Id.Equals(context.User.Identity.Name, StringComparison.InvariantCultureIgnoreCase));
                    var actor = user?.Actors.FirstOrDefault();

                    if (actor == null)
                    {
                        var rejection = new LoginRejected() { RejectionMessage = "User has no character.", NoCharacter = true };
                        Connection.SendMessage(socket, rejection);
                        await socket.CloseAsync(WebSocketCloseStatus.PolicyViolation, "User has no character", CancellationToken.None);
                        return;
                    }

                    if(Game.Connections.IsLoggedIn(user))
                    {
                        var rejection = new LoginRejected() { RejectionMessage = "User is already logged in." };
                        Connection.SendMessage(socket, rejection);
                        await socket.CloseAsync(WebSocketCloseStatus.PolicyViolation, "User is already logged in", CancellationToken.None);
                        return;
                    }

                    var connection = new Connection(socket, user, actor);
                    Game.Connections.Add(connection);
                    connection.SendMessage(new ServerMessage() { Contents = "Welcome to KatanaMUD. A MUD on the Web. Because I'm apparently insane. Dear lord." });

                    await HandleSocketCommunication(connection);
				}
				else
				{
					await next();
				}
			});

			app.UseStaticFiles();

			app.UseMvc(routes =>
			{
				routes.MapRoute("Default", "{controller=Home}/{action=Index}/{id?}");
			});
		}

		private async Task HandleSocketCommunication(Connection connection)
		{
			byte[] buffer = new byte[1024];
			WebSocketReceiveResult received = await connection.Socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

			while (!connection.Socket.CloseStatus.HasValue)
			{
                var message = MessageSerializer.DeserializeMessage(Encoding.UTF8.GetString(buffer, 0, received.Count));
                Game.MessageQueue.Enqueue(Tuple.Create(connection, message));

				received = await connection.Socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
			}

            Game.Connections.Remove(connection);
			await connection.Socket.CloseAsync(connection.Socket.CloseStatus.Value, connection.Socket.CloseStatusDescription, CancellationToken.None);
		}
	}
}
