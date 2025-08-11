//using Dapr.Client;

using Microsoft.AspNetCore.Mvc;
using ProductApi.Models;

namespace ProductService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        //private readonly DaprClient _daprClient;

        //public ProductController(DaprClient daprClient)
        //{
        //    _daprClient = daprClient;
        //}

        // GET: api/product
        [HttpGet]
        public IActionResult GetProducts()
        {
            // Simulate fetching products
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product A", Price = 10.99m, Stock = 100 },
                new Product { Id = 2, Name = "Product B", Price = 15.49m, Stock = 50 }
            };

            return Ok(products);
        }

        // GET: api/product/5
        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {
            // Simulate fetching a product by ID
            var product = new Product { Id = id, Name = "Product A", Price = 10.99m, Stock = 100 };

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // POST: api/product
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] Product product)
        {
            // Simulate product creation
            var productCreatedEvent = new
            {
                ProductId = product.Id,
                ProductName = product.Name,
                ProductPrice = product.Price
            };

            // Publish event to Dapr pub/sub
            //await _daprClient.PublishEventAsync("pubsub", "product-created", productCreatedEvent);

            return Ok(new { Message = "Product created and event published." });
        }

        // PUT: api/product/5
        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, [FromBody] Product product)
        {
            // Simulate product update
            if (id != product.Id)
            {
                return BadRequest("Product ID mismatch.");
            }

            // Here you would typically update the product in your data store

            return Ok(new { Message = "Product updated successfully." });
        }
    }
}
