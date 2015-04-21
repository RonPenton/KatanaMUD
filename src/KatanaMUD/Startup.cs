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
using Microsoft.AspNet.Http.Interfaces;

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

            app.UseWebSockets();

            app.Use(async (HttpContext context, Func<Task> next) =>
            {
                if (context.IsWebSocketRequest)
                {
                    var socket = await context.AcceptWebSocketAsync(context.WebSocketRequestedProtocols[0]);
                    var ip = context.GetFeature<IHttpConnectionFeature>()?.RemoteIpAddress?.ToString() ?? "No IP Address";
                    Console.WriteLine(String.Format("[{0}] Incoming connection: {1}", DateTime.Now.ToShortTimeString(), ip));

                    if (!context.User?.Identity.IsAuthenticated ?? true)
                    {
                        RejectConnection(socket, "User is not authenticated", ip);
                        return;
                    }

                    var user = Game.Data.Users.SingleOrDefault(x => x.Id.Equals(context.User.Identity.Name, StringComparison.InvariantCultureIgnoreCase));
                    var actor = user?.Actors.FirstOrDefault();

                    if (actor == null)
                    {
                        RejectConnection(socket, "User has no character.", ip);
                        return;
                    }

                    if (Game.Connections.IsLoggedIn(user))
                    {
                        Game.Connections.Disconnect(user);
                    }

                    var connection = Game.Connections.Connect(socket, user, actor, ip);

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

        private async void RejectConnection(WebSocket socket, string reason, string ip)
        {
            Console.WriteLine(String.Format("[{2}] Connection Aborted ({0}): {1}.", ip, reason, DateTime.Now.ToShortTimeString()));
            var rejection = new LoginRejected() { RejectionMessage = reason };
            ConnectionMessageHandler.HandleMessage(socket, rejection);
            await socket.CloseAsync(WebSocketCloseStatus.PolicyViolation, reason, CancellationToken.None);
        }

        private async Task HandleSocketCommunication(Connection connection)
        {
            try
            {
                byte[] buffer = new byte[1024];
                WebSocketReceiveResult received = await connection.Socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                while (!connection.Socket.CloseStatus.HasValue && !connection.Disconnected)
                {
                    var message = MessageSerializer.DeserializeMessage(Encoding.UTF8.GetString(buffer, 0, received.Count));
                    connection.Actor.AddMessage(message);

                    received = await connection.Socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                }

                Game.Connections.Disconnected(connection);

                if (connection.Socket.State != WebSocketState.Closed)
                {
                    await connection.Socket.CloseAsync(connection.Socket.CloseStatus.Value, connection.Socket.CloseStatusDescription, CancellationToken.None);
                }
            }
            catch (Exception)
            {
                Game.Connections.Disconnected(connection);
            }
        }
    }
}