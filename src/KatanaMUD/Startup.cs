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

			services.AddEntityFramework(Configuration)
				.AddSqlServer()
				.AddDbContext<GameContext>();

			//var builder = services.AddIdentity<User, Role>(Configuration);
			//builder.Services.Add(IdentityEntityFrameworkServices.GetDefaultServices(builder.UserType, builder.RoleType, typeof(GameContext)));
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
					WebSocket webSocket = await context.AcceptWebSocketAsync(context.WebSocketRequestedProtocols[0]);

                    Console.WriteLine("Incoming connection: " + context.Request.Host.Value);

                    // TODO: obviously refactor sending messages to be more streamlined.
                    var serverMessage = new ServerMessage() { Contents = "Welcome to KatanaMUD. A MUD on the Web. Because I'm apparently insane. Dear lord." };
                    var message = Encoding.UTF8.GetBytes(MessageSerializer.SerializeMessage(serverMessage));
                    await webSocket.SendAsync(new ArraySegment<byte>(message, 0, message.Length), WebSocketMessageType.Text, true, CancellationToken.None);

                    if (!context.User?.Identity.IsAuthenticated ?? true)
                    {
                        Console.WriteLine("Connection Aborted: Not Authorized.");
                        var rejection = new LoginRejected() { RejectionMessage = "User is not authenticated" };
                        message = Encoding.UTF8.GetBytes(MessageSerializer.SerializeMessage(rejection));
                        await webSocket.SendAsync(new ArraySegment<byte>(message, 0, message.Length), WebSocketMessageType.Text, true, CancellationToken.None);
                        await webSocket.CloseAsync(WebSocketCloseStatus.PolicyViolation, "User is not authenticated", CancellationToken.None);
                        return;
                    }

                    Game.Sockets.Add(webSocket);
                    await EchoWebSocket(webSocket, context);
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

		private async Task EchoWebSocket(WebSocket webSocket, HttpContext context)
		{
			byte[] buffer = new byte[1024];
			WebSocketReceiveResult received = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

			while (!webSocket.CloseStatus.HasValue)
			{
                var message = MessageSerializer.DeserializeMessage(Encoding.UTF8.GetString(buffer, 0, received.Count));
				Game.Messages.Enqueue(message);

				await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, received.Count), received.MessageType, received.EndOfMessage, CancellationToken.None);
				received = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
			}

			await webSocket.CloseAsync(webSocket.CloseStatus.Value, webSocket.CloseStatusDescription, CancellationToken.None);
		}

	}
}
