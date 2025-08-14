# Development steps
This documentation contain step by step instructions on OrderService microservice was build, run and deployed in Azure.

## Create the project
##### 1. Create a new ASP.NET Core Web API project from Visual Studio or using the .NET CLI.
   - If using Visual Studio, select "ASP.NET Core Web API" template.
   - If using .NET CLI, run:
	 ```powershell
	 dotnet new webapi -n OrderService
	 ```
##### 2. Implement the necessary models, controllers, and services for the OrderService API.
   - Create a `OrderCreatedEvent` that will be used to handle order creation events.
   - Create a `OrderController` to handle HTTP requests related to orders.
   - Implement methods for CRUD operations (Create, Read, Update, Delete) in the controller.

##### 3. Add Swagger for API documentation and testing.
   In the `OrderService` project, add the `Swashbuckle.AspNetCore` NuGet package to enable Swagger.
   - If using Visual Studio, right-click on the project, select "Manage NuGet Packages", and search for `Swashbuckle.AspNetCore`.
   - If using .NET CLI, run:
	 ```powershell
	 dotnet add package Swashbuckle.AspNetCore
	 ``` 
##### 4. Add Dapr Client for Dapr integration.
  In the `OrderService` project, you need to add the Dapr Client to enable communication with Dapr.
  Add the `Dapr.Client` NuGet package to the project.
   - If using Visual Studio, right-click on the project, select "Manage NuGet Packages", and search for `Dapr.Client`.
   - If using .NET CLI, run:
	 ```powershell
	 dotnet add package Dapr.Client
	 ```

##### 4. Replace code in Program.cs to configure Swagger and Daper. Open `Program.cs` and replace the existing code with the following:
```csharp
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddDapr();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

// Use this middleware to enable Dapr CloudEvents
app.UseCloudEvents();

app.UseAuthorization();

// This middleware must be added for Dapr to discover subscriptions
app.MapSubscribeHandler();


app.MapControllers();

app.Run();
```

## Containerization and run OrderService without Dapr
  
This chapter outlines the steps to containerize the OrderService API using Docker. The process includes building the Docker image, creating a self-signed certificate for HTTPS, and running the container with the necessary environment variables.
Open a terminal under solution folder and navigate to the OrderService project directory. The following steps will guide you through the containerization process:

##### 1. Build Immage
```powershell
docker build -t orderservice:latest .
```

##### 4. Run the Container
We will use the same self-signed certificate created for ProductService. Make sure you have the certificate file `productservice.pfx` in the `.aspnet\https` directory of your user profile.
```powershell
docker run -it --rm -p 8021:8021 `
  -e "ASPNETCORE_URLS=https://+:8021;http://+:8020" `
  -e "ASPNETCORE_HTTPS_PORTS=8021" `
  -e "ASPNETCORE_Kestrel__Certificates__Default__Path=/https/orderservice.pfx" `
  -e "ASPNETCORE_Kestrel__Certificates__Default__Password=runapifromdocker" `
  -v "${HOME}\.aspnet\https\orderservice.pfx:/https/orderservice.pfx" `
  orderservice
```

### Accessing the Order Service
You can access the Order Service API at the following URL:
```
https://localhost:8021/swagger/index.html
http://localhost:8020/swagger/index.html
```
## Local Run OrderService With Dapr
Open a terminal under solution folder and navigate to the OrderService project directory. 
Run the following command to start un the OrderService with Dapr:
```powershell
dapr run --app-id orderservice --app-port 5146 --components-path "../dapr/components" -- dotnet run
```

