using System.Globalization;
using EffectiveMobileTest1;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


Config config = Config.LoadConfig("config.json");

// Загрузка заказов из файла при запуске приложения
var orders = OrderService.LoadOrdersFromFile(config.InputFilePath);

// Вывести количество заказов в консоль для диагностики
Console.WriteLine($"Количество загруженных заказов: {orders.Count}");

app.MapGet("/orders", () => orders);

// Определение маршрута для фильтрации заказов
app.MapGet("/orders/filter1", (string district, DateTimeOffset from, DateTimeOffset to) =>
{

    // Фильтрация заказов
    var filteredOrders = OrderService.FilterOrders(orders, district, from, to);

    if (filteredOrders.Count == 0)
    {
        return Results.NotFound("No orders found matching the criteria.");
    }

    return Results.Ok(filteredOrders);
});

app.MapGet("/orders/filter2", (string district, DateTime firstDeliverytime) =>
{
    
    DateTime targetTime = firstDeliverytime.AddMinutes(30);

    // Фильтрация заказов
    var filteredOrders2 = OrderService.FilterOrders(orders, district, firstDeliverytime, targetTime);

    if (filteredOrders2.Count == 0)
    {
        return Results.NotFound("No orders found matching the criteria.");
    }

    return Results.Ok(filteredOrders2);
});

app.Run();
