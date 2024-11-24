using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json", true, true)
    .Build();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:4100", "http://localhost:4200")
            .WithHeaders("Content-Type", "Authorization")
            .WithMethods("GET", "POST", "PUT", "DELETE");
    });
});

builder.Services.AddOcelot(configuration);

var app = builder.Build();

app.UseCors();

await app.UseOcelot();

app.MapGet("/", () => "healthy");

app.Run();

