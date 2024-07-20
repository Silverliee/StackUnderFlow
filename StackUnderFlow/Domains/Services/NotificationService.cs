using System.Net.WebSockets;
using System.Text;
using System.Collections.Concurrent;

namespace StackUnderFlow.Domains.Services;

public class NotificationService : INotificationService
{
    private readonly ConcurrentDictionary<string, WebSocket> _sockets = new();

    public async Task HandleAsync(HttpContext context, string id)
    {
        if (context.WebSockets.IsWebSocketRequest)
        {
            var webSocket = await context.WebSockets.AcceptWebSocketAsync();
            _sockets[id] = webSocket;
            await KeepConnectionOpenAsync(id, webSocket, context.RequestAborted);
        }
        else
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync("Not a WebSocket request.");
        }
    }

    private async Task KeepConnectionOpenAsync(string id, WebSocket webSocket, CancellationToken cancellationToken)
    {
        var buffer = new byte[1024 * 4];
        try
        {
            while (webSocket.State == WebSocketState.Open)
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await CloseConnectionAsync(id);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception occurred: {ex.Message}");
            await CloseConnectionAsync(id);
        }
    }

    public async Task SendMessageAsync(string id, string message)
    {
        if (_sockets.TryGetValue(id, out var webSocket) && webSocket.State == WebSocketState.Open)
        {
            var buffer = Encoding.UTF8.GetBytes(message);
            await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
    
    public async Task CloseConnectionAsync(string id)
    {
        if (_sockets.TryGetValue(id, out var webSocket))
        {
            if (webSocket.State == WebSocketState.Open)
            {
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by server", CancellationToken.None);
            }
            _sockets.TryRemove(id, out _);
        }
    }
}
