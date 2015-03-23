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
using KatanaMUD.Authorization;
using Microsoft.AspNet.Security.Cookies;

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
			var thread = new Thread(GameLoop.Run);
			thread.Start();

			app.UseCookieAuthentication(opts =>
			{
				opts.AuthenticationType = CookieAuthenticationDefaults.AuthenticationType;
				opts.LoginPath = new PathString("/Account/Login");
			});

			app.Use(async (context, next) =>
			{
				if (context.IsWebSocketRequest)
				{
					WebSocket webSocket = await context.AcceptWebSocketAsync(context.WebSocketRequestedProtocols[0]);
					await EchoWebSocket(webSocket);
				}
				else
				{
					await next();
				}
			});

			app.UseMvc(routes =>
			{
				routes.MapRoute("Default", "{controller=Home}/{action=Index}/{id?}");
			});
		}

		private async Task EchoWebSocket(WebSocket webSocket)
		{
			byte[] buffer = new byte[1024];
			WebSocketReceiveResult received = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

			while (!webSocket.CloseStatus.HasValue)
			{
				var message = Encoding.UTF8.GetString(buffer, 0, received.Count);
				GameLoop.Messages.Enqueue(message);

				await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, received.Count), received.MessageType, received.EndOfMessage, CancellationToken.None);
				received = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
			}

			await webSocket.CloseAsync(webSocket.CloseStatus.Value, webSocket.CloseStatusDescription, CancellationToken.None);
		}

	}
}
