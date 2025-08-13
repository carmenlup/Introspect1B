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
