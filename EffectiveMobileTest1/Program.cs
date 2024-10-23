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


// ���� � Json �����
string filePath = "orders.json";

// �������� ������� �� ����� ��� ������� ����������
var orders = OrderService.LoadOrdersFromFile(filePath);

// ������� ���������� ������� � ������� ��� �����������
Console.WriteLine($"���������� ����������� �������: {orders.Count}");

app.MapGet("/orders", () => orders);

// ����������� �������� ��� ���������� �������
app.MapGet("/orders/filter", (string district, DateTimeOffset from, DateTimeOffset to) =>
{

    // ���������� �������
    var filteredOrders = OrderService.FilterOrders(orders, district, from, to);

    if (filteredOrders.Count == 0)
    {
        return Results.NotFound("No orders found matching the criteria.");
    }

    return Results.Ok(filteredOrders);
});

app.Run();
