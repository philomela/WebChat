using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace WebChat.Services
{
    public interface IWebSocketChat
    {
        List<WebSocket> WebSockets { get; }
        void AddWebSocket(WebSocket webSocket);
        Task Echo(HttpContext context, WebSocket currWebSocket);
    }
}
