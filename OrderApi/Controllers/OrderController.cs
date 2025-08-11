using Microsoft.AspNetCore.Mvc;

namespace OrderApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        [HttpPost("product-created")]
        public IActionResult HandleProductCreated([FromBody] dynamic productCreatedEvent)
        {
            // Handle the event (e.g., create an order for the product)
            Console.WriteLine($"OrderService received event: Product {productCreatedEvent.ProductName} created with price {productCreatedEvent.ProductPrice}");
            return Ok();
        }
    }
}
