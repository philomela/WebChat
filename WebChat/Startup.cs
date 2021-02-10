using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebChat.Services;

namespace WebChat
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IWebSocketChat, WebSocketChat>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IWebSocketChat webScktChat)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseWebSockets();
            //var arrayConnections = new List<WebSocket>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/GetSocket", async context =>
                {
                    using (WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync())
                    {
                        await webScktChat.Echo(context, webSocket);
                        //await Echo(context, webSocket);
                    }
                });
                endpoints.MapGet("/Index", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });

            /*async Task Echo(HttpContext context, WebSocket currWebSocket)
            {

                arrayConnections.Add(currWebSocket);
                var buffer = new byte[1024 * 4];

                string administration = JsonConvert.SerializeObject(new { Name = "Roman", Age = 26, Company = "Renessaince" });
                buffer = Encoding.Default.GetBytes(administration);

                while (currWebSocket.State == WebSocketState.Open)
                {
                    await currWebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                    foreach (var currentSoscket in arrayConnections)
                        if (currentSoscket.State == WebSocketState.Open)
                            await currentSoscket.SendAsync(new ArraySegment<byte>(buffer, 0, buffer.Length), WebSocketMessageType.Text, true, CancellationToken.None);

                }
            }*/
        }
    }
}
