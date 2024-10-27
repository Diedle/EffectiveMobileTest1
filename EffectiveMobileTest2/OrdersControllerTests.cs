using EffectiveMobileTest1.Controllers;
using EffectiveMobileTest1.Models;
using EffectiveMobileTest1.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EffectiveMobileTest2
{
    public class OrdersControllerTests
    {
        private readonly Mock<IOrderFilter> _mockOrderFilter;
        private readonly OrdersController _controller;

        public OrdersControllerTests()
        {
            _mockOrderFilter = new Mock<IOrderFilter>();
            _controller = new OrdersController(_mockOrderFilter.Object);
        }

        [Fact]
        public void FilterOrdersFromTo_ReturnsNotFound_WhenNoOrdersMatch()
        {
            // Arrange
            _mockOrderFilter.Setup(filter => filter.FilterOrdersFromTo(It.IsAny<string>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>()))
                .Returns(new List<Order>());

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
            new Order("1", 10, "District1", DateTimeOffset.UtcNow)
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
                .Returns(new List<Order>());

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
            new Order("1", 10, "District1", new DateTimeOffset(2024, 11, 11, 10, 15, 0, TimeSpan.Zero))
            };

            _mockOrderFilter.Setup(filter => filter.FilterOrdersTargetTime(It.IsAny<string>(), It.IsAny<DateTime>()))
                .Returns(mockOrders);

            // Act
            var result = _controller.FilterOrdersTargetTime("District1", new DateTime(2024, 11, 11, 10, 0, 0));

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(mockOrders, okResult.Value);
        }
    }
}