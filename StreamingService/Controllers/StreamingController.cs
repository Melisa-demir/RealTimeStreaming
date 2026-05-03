using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary;

namespace StreamingService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StreamingController : ControllerBase
    {
        private readonly RabbitMqHelper _rabbitMqHelper;
        public StreamingController(RabbitMqHelper rabbitMqHelper)
        {
            _rabbitMqHelper = rabbitMqHelper;
        }

        [HttpPost("send")]
        public IActionResult SendMessage([FromBody] string message)
        {
            _rabbitMqHelper.PublishMessage("notification-queue", message);
            return Ok(new { Status = "Message sent to RabbitMQ" });
        }
    }
}
