using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        public const string Url = "https://fcm.googleapis.com/fcm/send";
    }
}