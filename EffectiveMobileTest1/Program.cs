using EffectiveMobileTest1.Middleware;
using EffectiveMobileTest1.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


LoggerConfiguration();

ConfigureServices();

ConfigureApp();


void LoggerConfiguration()
{
    Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("_deliveryLog.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

     builder.Host.UseSerilog();
}

void ConfigureServices()
{
    builder.Configuration
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json");

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddControllers();
    builder.Services.AddSingleton<IFilterOrdersService, FilterOrdersService>();
    builder.Services.AddSingleton<IFileManager, FileManager>();
    builder.Services.AddSingleton<IOrderHandler, OrderHandler>();
    builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
    builder.Services.AddMemoryCache();
}

void ConfigureApp()
{
    var app = builder.Build();

    app.UseMiddleware<ApiKeyMiddleware>();
    app.UseMiddleware<ExceptionHandlingMiddleware>();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.MapControllers();

    app.UseRouting();

    app.UseAuthorization();

    app.UseEndpoints(endpoints =>
    {
        _ = endpoints.MapControllers();
    });

    app.Run();
}