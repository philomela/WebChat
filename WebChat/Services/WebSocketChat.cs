using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebChat.Services
{
    public class WebSocketChat : IWebSocketChat
    {
        public WebSocketChat()
        {
            WebSockets = new List<WebSocket>();
        }
        public List<WebSocket> WebSockets { get; }
        public void AddWebSocket(WebSocket webSocket) => WebSockets?.Add(webSocket);
        public async Task Echo(HttpContext context, WebSocket currWebSocket)
        {
            WebSockets.Add(currWebSocket);
            var buffer = new byte[1024 * 4];

            string administration = JsonConvert.SerializeObject(new { Name = "Roman", Age = 26, Company = "Renessaince" });
            buffer = Encoding.Default.GetBytes(administration);

            while (currWebSocket.State == WebSocketState.Open)
            {
                await currWebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                foreach (var currentSoscket in WebSockets)
                    if (currentSoscket.State == WebSocketState.Open)
                        await currentSoscket.SendAsync(new ArraySegment<byte>(buffer, 0, buffer.Length), WebSocketMessageType.Text, true, CancellationToken.None);

            }
        }
    }
}
