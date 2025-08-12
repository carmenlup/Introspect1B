# Development steps
This documentation contain step by step instructions on ProductService microservice was build, run and deployed in Azure.

## Create the project
1. Create a new ASP.NET Core Web API project from Visual Studio or using the .NET CLI.
   - If using Visual Studio, select "ASP.NET Core Web API" template.
   - If using .NET CLI, run:
	 ```
	 dotnet new webapi -n ProductService
	 ```
2. Implement the necessary models, controllers, and services for the ProductService API.
   - Create a `Product` model with properties like `Id`, `Name`, `Description`, `Price`, etc.
   - Create a `ProductsController` to handle HTTP requests related to products.
   - Implement methods for CRUD operations (Create, Read, Update, Delete) in the controller.

3. Add Swagger for API documentation and testing.
   In the `ProductService` project, add the `Swashbuckle.AspNetCore` NuGet package to enable Swagger.
   - If using Visual Studio, right-click on the project, select "Manage NuGet Packages", and search for `Swashbuckle.AspNetCore`.
   - If using .NET CLI, run:
	 ```
	 dotnet add package Swashbuckle.AspNetCore
	 ``` 
4 Add Dapr for service invocation and state management.
  Add the `Dapr.Client` NuGet package to the project.
   - If using Visual Studio, right-click on the project, select "Manage NuGet Packages", and search for `Dapr.Client`.
   - If using .NET CLI, run:
	 ```
	 dotnet add package Dapr.Client
	 ```
5. Configure Swagger and Dapr in Program.cs. Open `Program.cs` and replace the existing code with the following:

	```csharp
	using Dapr.Client;

	var builder = WebApplication.CreateBuilder(args);

	// Add services to the container.

	builder.Services.AddControllers(); 
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

6. Update ProductService Create endpoint with dapr service invocation. Open the `ProductsController.cs` file and modify the `CreateProduct` method to use Dapr for service invocation:
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
		// Use Dapr to invoke another service or perform state management
		await _daprClient.InvokeMethodAsync("other-service", "create-product", product);
		
		// Logic to save the product in the database or in-memory store
		return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
	}
	```
The above code demonstrates how to use Dapr for service invocation in the `CreateProduct` method. You can similarly update other methods to use Dapr for state management or service invocation as needed.
Reffer to [Program.cs](ProductService/Program.cs) for complete code of the ProductService API.
