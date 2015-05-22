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
            var exp = Exp.calcExpNeededOld(0, 0);
            exp = Exp.calcExpNeededOld(1, 0);
            exp = Exp.calcExpNeededOld(2, 0);
            exp = Exp.calcExpNeededOld(3, 0);
            exp = Exp.calcExpNeededOld(50, 0);

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


    public class Exp
    {
        static uint[] expModTable = {
               1,  1, 40, 20, 44, 24, 44, 24, 48, 28, 48, 28, 52,
              32, 52, 32, 56, 36, 56, 36, 60, 40, 60, 40, 65, 45,
              65, 45, 70, 50, 70, 50, 75, 55, 50, 40, 50, 40, 50,
              40, 50, 40, 50, 40, 50, 40, 50, 40, 50, 40, 23, 20,
              23, 20, 23, 20, 23, 20, 23, 20, 23, 20, 23, 20 };

        public static uint calcExpNeededOld(uint level, uint chart)
        {
            uint res = 0,
                i = 0,
                scalemul = 0,
                scalediv = 0;

            res = ((chart * 1000) + 100000) / 100;

            while (i < level)
            {
                if (i < 26)
                {
                    scalemul = expModTable[i * 2];
                    scalediv = expModTable[(i * 2) + 1];
                }
                else if (i > 60)
                {
                    scalemul = 105;
                    scalediv = 100;
                }
                else if (i > 52)
                {
                    scalemul = 110;
                    scalediv = 100;
                }
                else
                {
                    scalemul = 115;
                    scalediv = 100;
                }

                if ((res <= (res * scalemul)) && (((res * scalemul) / scalemul) == res))
                {
                    res = (res * scalemul) / scalediv;
                }
                else
                {
                    res = res / 100;
                    if ((res <= (res * scalemul)) && (((res * scalemul) / scalemul) == res))
                        res = (res * scalemul) / scalediv;
                    else
                        res = (((res / 100) * scalemul) / scalediv) * 100;
                    res = res * 100;
                }
                i++;
            }

            return res;
        }
    }
}