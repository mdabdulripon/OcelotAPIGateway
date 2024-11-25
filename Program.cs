using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Load Ocelot configuration based on environment
IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json", true, true)
    .Build();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:4100", "http://localhost:4200")
            .AllowAnyHeader()    // Allow any headers like Authorization
            .AllowAnyMethod();   // Allow any HTTP methods (GET, POST, PUT, DELETE)
    });
});

// Register Ocelot
builder.Services.AddOcelot(configuration);

var app = builder.Build();

// Enable CORS before Ocelot middleware
app.UseCors();

// Use Ocelot API Gateway Middleware
await app.UseOcelot();

app.MapGet("/", () => "healthy");

app.Run();