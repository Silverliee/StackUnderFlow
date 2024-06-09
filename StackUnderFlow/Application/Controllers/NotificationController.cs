using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;

namespace StackUnderFlow.Application.Controllers;

[ApiController]
[Route("[controller]")] 
[EnableCors("AllowAll")]
public class NotificationController : ControllerBase
{
    
}