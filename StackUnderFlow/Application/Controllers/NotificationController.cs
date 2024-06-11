using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace StackUnderFlow.Application.Controllers;

[ApiController]
[Route("[controller]")]
[EnableCors("AllowAll")]
public class NotificationController : ControllerBase { }
