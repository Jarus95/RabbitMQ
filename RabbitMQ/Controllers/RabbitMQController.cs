using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Models;
using RabbitMQ.Services;

namespace RabbitMQ.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RabbitMQController : ControllerBase
    {
        private readonly IRabbitService _rabbitService;

        public RabbitMQController(IRabbitService rabbitService)
        {
            _rabbitService = rabbitService;
        }

        [HttpPost]
        public IActionResult SendMessage(StudentDto dto)
        {
            _rabbitService.Send(dto);
            return Ok();
        }

        [HttpGet]
        public IActionResult GetMessages() 
        { 
           var data = _rabbitService.GetStudents();
            return Ok(data);
        }
    }
}
