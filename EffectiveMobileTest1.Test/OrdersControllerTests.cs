using Moq;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;

namespace EffectiveMobileTest1.Test
{
    public class OrdersControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private HttpClient _client;

        public OrdersControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task FilterOrders_ReturnsNotFound_WhenNoOrdersMatch()
        {
            // Arrange
            var orders = new List<Order>(); // Пустой список заказов
            var district = "DistrictA";
            var from = new DateTimeOffset(2024, 11, 11, 10, 0, 0, TimeSpan.Zero);
            var to = new DateTimeOffset(2024, 11, 11, 10, 30, 0, TimeSpan.Zero);

            var mockOrderService = new Mock<MockOrderService>();
            mockOrderService
                .Setup(service => service.FilterOrders(orders, district, from, to))
                .Returns(new List<Order>());

            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddSingleton(mockOrderService.Object);
                });
            }).CreateClient();

            // Act
            var response = await client.GetAsync($"/orders/filter1?district={district}&from={from:O}&to={to:O}");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task FilterOrders_ReturnsOk_WhenOrdersMatch()
        {
            // Arrange
            var orders = new List<Order>
            {
            new Order (  "1",  10, "DistrictA", new DateTimeOffset(2024, 11, 11, 10, 15, 0, TimeSpan.Zero) )
            };

            var district = "DistrictA";
            var from = new DateTimeOffset(2024, 11, 11, 10, 0, 0, TimeSpan.Zero);
            var to = new DateTimeOffset(2024, 11, 11, 10, 30, 0, TimeSpan.Zero);

            var mockOrderService = new Mock<MockOrderService>();
            mockOrderService
                .Setup(service => service.FilterOrders(orders, district, from, to))
                .Returns(orders);

            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddSingleton(mockOrderService.Object);
                });
            }).CreateClient();

            // Act
            var response = await client.GetAsync($"/orders/filter1?district={district}&from={from:O}&to={to:O}");
            var responseContent = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.Contains("1", responseContent);
        }
    }

    public class MockOrderService
    {
        public virtual List<Order> FilterOrders(List<Order> orders, string district, DateTimeOffset from, DateTimeOffset to)
        {
            return OrderService.FilterOrders(orders, district, from, to);
        }
    }
}