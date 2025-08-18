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

// Use this middleware to enable Dapr CloudEvents
app.UseCloudEvents();

app.UseAuthorization();

app.MapControllers();

app.Run();
