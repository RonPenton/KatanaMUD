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
using Microsoft.AspNet.StaticFiles;
using KatanaMUD.Messages;
using System.Linq;
using Microsoft.AspNet.Authentication.Cookies;
using Microsoft.AspNet.Http.Features;

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
                opts.AuthenticationScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                opts.LoginPath = new PathString("/Account/Login");
                opts.AutomaticAuthentication = true;
            });

            app.UseWebSockets();

            app.Use(async (HttpContext context, Func<Task> next) =>
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    var socket = await context.WebSockets.AcceptWebSocketAsync(context.WebSockets.WebSocketRequestedProtocols[0]);
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
                        //TODO: Tell user why they are disconnected.
                        Game.Connections.Disconnect(user);
                    }

                    var connection = Game.Connections.Connect(socket, user, actor, ip);

                    await connection.HandleSocketCommunication();
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
            await ConnectionMessageHandler.HandleMessage(socket, rejection);
            await socket.CloseAsync(WebSocketCloseStatus.PolicyViolation, reason, CancellationToken.None);
        }
    }


}