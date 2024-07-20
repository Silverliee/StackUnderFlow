using System.Net.WebSockets;

namespace StackUnderFlow.Domains.Services;

public interface INotificationService
{
    public Task HandleAsync(HttpContext context, string id);
    public Task SendMessageAsync(string id, string message);
    public Task CloseConnectionAsync(string id);
}
