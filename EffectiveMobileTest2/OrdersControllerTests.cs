using EffectiveMobileTest1.Controllers;
using EffectiveMobileTest1.Models;
using EffectiveMobileTest1.Services;
using EffectiveMobileTest1.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace EffectiveMobileTest2
{
    public class OrdersControllerTests
    {
        private readonly Mock<IOrderHandler> _mockOrderFilter;
        private readonly Mock<ILogger<OrdersController>> _mockLogger;
        private readonly OrdersController _controller;
        private readonly FilterOrdersService _service;

        public OrdersControllerTests()
        {
            _mockOrderFilter = new Mock<IOrderHandler>();
            _mockLogger = new Mock<ILogger<OrdersController>>();
            _controller = new OrdersController(_mockOrderFilter.Object, _mockLogger.Object);
            _service = new FilterOrdersService();
        }

        [Fact]
        public void FilterOrdersFromTo_ReturnsNotFound_WhenNoOrdersMatch()
        {
            // Arrange
            _mockOrderFilter.Setup(filter => filter.FilterOrdersFromTo(It.IsAny<string>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>()))
                .Returns([]);

            // Act
            var result = _controller.FilterOrdersFromTo("District1", DateTimeOffset.UtcNow.AddDays(-1), DateTimeOffset.UtcNow.AddDays(1));

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("No orders found matching the criteria.", notFoundResult.Value);
        }

        [Fact]
        public void FilterOrdersFromTo_ReturnsOk_WhenOrdersMatch()
        {
            // Arrange
            var mockOrders = new List<Order>
            {
                new ("1", 10, "District1", DateTimeOffset.UtcNow)
            };

            _mockOrderFilter.Setup(filter => filter.FilterOrdersFromTo(It.IsAny<string>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>()))
                .Returns(mockOrders);

            // Act
            var result = _controller.FilterOrdersFromTo("District1", DateTimeOffset.UtcNow.AddDays(-1), DateTimeOffset.UtcNow.AddDays(1));

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(mockOrders, okResult.Value);
        }

        [Fact]
        public void FilterOrdersTargetTime_ReturnsNotFound_WhenNoOrdersMatch()
        {
            // Arrange
            _mockOrderFilter.Setup(filter => filter.FilterOrdersTargetTime(It.IsAny<string>(), It.IsAny<DateTime>()))
                .Returns([]);

            // Act
            var result = _controller.FilterOrdersTargetTime("District1", new DateTime(2024, 11, 11, 10, 0, 0));

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("No orders found matching the criteria.", notFoundResult.Value);
        }

        [Fact]
        public void FilterOrdersTargetTime_ReturnsOk_WhenOrdersMatch()
        {
            // Arrange
            var mockOrders = new List<Order>
            {
                new ("1", 10, "District1", new DateTimeOffset(2024, 11, 11, 10, 15, 0, TimeSpan.Zero))
            };

            _mockOrderFilter.Setup(filter => filter.FilterOrdersTargetTime(It.IsAny<string>(), It.IsAny<DateTime>()))
                .Returns(mockOrders);

            // Act
            var result = _controller.FilterOrdersTargetTime("District1", new DateTime(2024, 11, 11, 10, 0, 0));

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(mockOrders, okResult.Value);
        }

        [Fact]
        public void FilterOrders_ReturnsOrdersInDistrictAndTimeRange()
        {
            // Arrange
            var orders = new List<Order>
            {
                new ("1", 10, "District1", new DateTimeOffset(2023, 1, 1, 10, 0, 0, TimeSpan.Zero)),
                new ("2", 15, "District1", new DateTimeOffset(2023, 1, 1, 12, 0, 0, TimeSpan.Zero)),
                new ("3", 20, "District2", new DateTimeOffset(2023, 1, 1, 11, 0, 0, TimeSpan.Zero)),
            };
            var district = "District1";
            var from = new DateTimeOffset(2023, 1, 1, 9, 0, 0, TimeSpan.Zero);
            var to = new DateTimeOffset(2023, 1, 1, 11, 0, 0, TimeSpan.Zero);

            // Act
            var result = _service.FilterOrders(orders, district, from, to);

            // Assert
            Assert.NotEmpty(result);
            Assert.Single(result);
            Assert.Equal("1", result[0].OrderId);
        }

        [Fact]
        public void FilterOrders_ReturnsEmptyList_WhenNoOrdersMatch()
        {
            // Arrange
            var orders = new List<Order>
            {
                new ("1", 10, "District1", new DateTimeOffset(2023, 1, 1, 10, 0, 0, TimeSpan.Zero)),
                new ("2", 15, "District1", new DateTimeOffset(2023, 1, 1, 12, 0, 0, TimeSpan.Zero)),
                new ("3", 20, "District2", new DateTimeOffset(2023, 1, 1, 11, 0, 0, TimeSpan.Zero)),
            };
            var district = "District3";
            var from = new DateTimeOffset(2023, 1, 1, 9, 0, 0, TimeSpan.Zero);
            var to = new DateTimeOffset(2023, 1, 1, 11, 0, 0, TimeSpan.Zero);

            // Act
            var result = _service.FilterOrders(orders, district, from, to);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void FilterOrders_ReturnsOrdersCaseInsensitive()
        {
            // Arrange
            var orders = new List<Order>
            {
                new ("1", 10, "District1", new DateTimeOffset(2023, 1, 1, 10, 0, 0, TimeSpan.Zero)),
                new ("2", 15, "District1", new DateTimeOffset(2023, 1, 1, 12, 0, 0, TimeSpan.Zero)),
                new ("3", 20, "District2", new DateTimeOffset(2023, 1, 1, 11, 0, 0, TimeSpan.Zero)),
            };
            var district = "district1";
            var from = new DateTimeOffset(2023, 1, 1, 9, 0, 0, TimeSpan.Zero);
            var to = new DateTimeOffset(2023, 1, 1, 11, 0, 0, TimeSpan.Zero);

            // Act
            var result = _service.FilterOrders(orders, district, from, to);

            // Assert
            Assert.NotEmpty(result);
            Assert.Single(result);
            Assert.Equal("1", result[0].OrderId);
        }

        [Fact]
        public void FilterOrders_ReturnsOrdersWithinTimeRange()
        {
            // Arrange
            var orders = new List<Order>
            {
                new ("1", 10, "District1", new DateTimeOffset(2023, 1, 1, 10, 0, 0, TimeSpan.Zero)),
                new ("2", 15, "District1", new DateTimeOffset(2023, 1, 1, 12, 0, 0, TimeSpan.Zero)),
                new ("3", 20, "District2", new DateTimeOffset(2023, 1, 1, 11, 0, 0, TimeSpan.Zero)),
            };
            var district = "District1";
            var from = new DateTimeOffset(2023, 1, 1, 10, 0, 0, TimeSpan.Zero);
            var to = new DateTimeOffset(2023, 1, 1, 12, 0, 0, TimeSpan.Zero);

            // Act
            var result = _service.FilterOrders(orders, district, from, to);

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, o => o.OrderId == "1");
            Assert.Contains(result, o => o.OrderId == "2");
        }
    }
}