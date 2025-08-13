using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Dapr;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        [Topic("pubsub", "product-created")]
        [HttpPost("product-created")]
        public IActionResult HandleProductCreated([FromBody] ProductCreatedEvent productCreatedEvent)
        {
            // Serialize the object to a JSON string
            var jsonPayload = JsonSerializer.Serialize(productCreatedEvent);

            // Log the full JSON payload
            Console.WriteLine($"OrderService received event: {jsonPayload}");
            return Ok(productCreatedEvent);
        }
    }
}
