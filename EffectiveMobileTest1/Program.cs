using EffectiveMobileTest1.Middleware;
using EffectiveMobileTest1.Models;
using EffectiveMobileTest1.Services;


var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json");
// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();
builder.Services.AddSingleton<IOrderService, OrderService>();
builder.Services.AddSingleton<IFileManager, FileManager>();
builder.Services.AddSingleton<IOrderFilter, OrderFilter>();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddMemoryCache();


var app = builder.Build();

app.UseMiddleware<JsonValidationMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

// Настройка маршрутизации
app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    _ = endpoints.MapControllers();
});

app.Run();
public partial class Program { }