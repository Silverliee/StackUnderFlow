using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using StackUnderFlow.Domains.Services;

namespace StackUnderFlow.Application.Controllers;

[ApiController]
[Route("[controller]")]
[EnableCors("AllowAll")]
public class NotificationController(INotificationService notificationService) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<OkResult> Get(string id)
    {
        await notificationService.HandleAsync(HttpContext, id);
        return Ok();
    }
}