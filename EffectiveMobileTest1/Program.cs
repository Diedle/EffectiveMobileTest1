using EffectiveMobileTest1.Middleware;
using EffectiveMobileTest1.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog for logging
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("_deliveryLog.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Set the configuration source to appsettings.json
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json");

// Add essential services to the DI container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

// Register custom services for dependency injection
builder.Services.AddSingleton<IFilterOrdersService, FilterOrdersService>();
builder.Services.AddSingleton<IFileManager, FileManager>();
builder.Services.AddSingleton<IOrderHandler, OrderHandler>();

// Register configuration and caching services
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddMemoryCache();

var app = builder.Build();

// Use custom middleware for handling exceptions
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Enable Swagger in development environment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.UseRouting();

app.UseAuthorization();

// Map endpoints to controllers
app.UseEndpoints(endpoints =>
{
    _ = endpoints.MapControllers();
});

app.Run();