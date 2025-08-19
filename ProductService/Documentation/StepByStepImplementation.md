# Development steps
This documentation contain step by step instructions on ProductService microservice was build, run and deployed Docker. 
Using this documentation you can build the ProductServvie microservice from scratch and run it locally with Dapr or without Dapr.

## Create the project
##### 1. Create a new ASP.NET Core Web API project from Visual Studio or using the .NET CLI.
   - If using Visual Studio, select "ASP.NET Core Web API" template.
   - If using .NET CLI, run:
	 ```
	 dotnet new webapi -n ProductService
	 ```
##### 2. Implement the necessary models, controllers, and services for the ProductService API.
   - Create a `Product` model with properties like `Id`, `Name`, `Description`, `Price`, etc.
   - Create a `ProductsController` to handle HTTP requests related to products.
   - Implement methods for CRUD operations (Create, Read, Update, Delete) in the controller.

##### 3. Add Swagger for API documentation and testing.
   In the `ProductService` project, add the `Swashbuckle.AspNetCore` NuGet package to enable Swagger.
   - If using Visual Studio, right-click on the project, select "Manage NuGet Packages", and search for `Swashbuckle.AspNetCore`.
   - If using .NET CLI, run:
	 ```
	 dotnet add package Swashbuckle.AspNetCore
	 ``` 
##### 4. Replace code in Program.cs to configure Swagger and enable HTTPS redirection. Open `Program.cs` and replace the existing code with the following:

``` csharp
using Dapr.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(); 
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
```

## Containerization and run ProductService without Dapr
  
This chapter outlines the steps to containerize the ProductService API using Docker. The process includes building the Docker image, creating a self-signed certificate for HTTPS, and running the container with the necessary environment variables.
Open a terminal under solution folder and navigate to the ProductService project directory. The following steps will guide you through the containerization process:

##### 1. Build Immage
```powershell
docker build -t productservice:latest .
```

##### 2. Create  Self-Signed Certificate
```powershell
dotnet dev-certs https -t -ep "%USERPROFILE%\.aspnet\https\productservice.pfx" -p runapifromdocker
```
##### 3. Trust the certificate on your local machine
```powershell
dotnet dev-certs https --trust
```

##### 4. Run the Container

```powershell
docker run -it --rm -p 8081:8081 `
  -e "ASPNETCORE_URLS=https://+:8081;http://+:8080" `
  -e "ASPNETCORE_HTTPS_PORTS=8081" `
  -e "ASPNETCORE_Kestrel__Certificates__Default__Path=/https/productservice.pfx" `
  -e "ASPNETCORE_Kestrel__Certificates__Default__Password=runapifromdocker" `
  -v "${HOME}\.aspnet\https\productservice.pfx:/https/productservice.pfx" `
  productservice
```

### Accessing the Product Service
You can access the Product Service API at the following URL:
```
https://localhost:8081/swagger/index.html
http://localhost:8080/swagger/index.html
```
## Implement Dapr in ProductService
##### 1. Add Dapr for service invocation and state management.
  Add the `Dapr.Client` NuGet package to the project.
   - If using Visual Studio, right-click on the project, select "Manage NuGet Packages", and search for `Dapr.Client`.
   - If using .NET CLI, run:
	 ```powershell
	 dotnet add package Dapr.Client
	 ```
##### 2. Configure Dapr in Program.cs. Open `Program.cs` and replace the existing code with the following:

```csharp
 using Dapr.Client;
 
 var builder = WebApplication.CreateBuilder(args);
 
 // Add services to the container.
 
 builder.Services.AddControllers().AddDapr(); 
 // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
 builder.Services.AddEndpointsApiExplorer();
 
 builder.Services.AddSwaggerGen();
 
 // Register DaprClient with the dependency injection container.
 builder.Services.AddSingleton(new DaprClientBuilder().Build());
 
 var app = builder.Build();
 
 // Configure the HTTP request pipeline.
 app.UseSwagger();
 app.UseSwaggerUI();
 
 app.UseHttpsRedirection();
 
 app.UseAuthorization();
 
 app.MapControllers();
 
 app.Run();
```

##### 3. Update ProductService Create endpoint with dapr service invocation. Open the `ProductsController.cs` file and modify the `CreateProduct` method to use Dapr for service invocation:
- Inject `DaprClient` into the controller.
	```csharp
	private readonly DaprClient _daprClient;
    
	public ProductController(DaprClient daprClient)
	{
		_daprClient = daprClient;
	}
	```
- Update the `CreateProduct` method to invoke the Dapr service:
	```csharp
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

		// Add this line to log the event before publishing
		var jsonPayload = JsonSerializer.Serialize(productCreatedEvent);
		Console.WriteLine($"ProductService is sending event: {jsonPayload}");

		// Publish event to Dapr pub/sub
		await _daprClient.PublishEventAsync("pubsub", "product-created", productCreatedEvent);

		return Ok(new { Message = $"Product {productCreatedEvent.ProductName} was created and event published." });
	}
	```
The above code demonstrates how to use Dapr for service invocation in the `CreateProduct` method. You can similarly update other methods to use Dapr for state management or service invocation as needed.
Reffer to [Program.cs](../Program.cs) for complete code of the ProductService API.


## Local Run ProductService With Dapr
Open a terminal under solution folder and navigate to the ProductService project directory. 
Run the following command to start the ProductService with Dapr:
```powershell
dapr run --app-id orderservice --app-port 5125 --components-path "../dapr/components" -- dotnet run
```
This command starts the ProductService application with Dapr, allowing it to communicate with other services and utilize Dapr features.

## Accessing the Product Service with Dapr
You can use Swagger UI to test the ProductService API endpoints. Open your web browser and navigate to:
```
https://localhost:5125/swagger/index.html
```

## Test communication between ProductService and OrderService using Dapr on local
You can test the communication between ProductService and OrderService using Dapr by invoking the endpoints defined in the ProductService API. 
In Swagger UI, go to create endpoint and create a product. After that, you can check the OrderService logs to see if the event was received.
### Example HTTP Requests for Create a Product
```http
POST https://localhost:5125/api/products
Content-Type: application/json

{
  "id": 1,
  "name": "Sample Product",
  "description": "This is a sample product.",
  "price": 19.99
}
```
The above request creates a new product in the ProductService. 
After the product is created, the ProductService will publish an event to Dapr pub/sub, which can be consumed by the OrderService and a message with product details will be logged in the OrderService console.
