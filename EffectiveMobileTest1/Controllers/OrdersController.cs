using EffectiveMobileTest1.Constants;
using EffectiveMobileTest1.Models;
using EffectiveMobileTest1.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EffectiveMobileTest1.Controllers
{
    /// <summary>
    /// API controller for filtering orders.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderHandler _orderHandler;
        private readonly ILogger<OrdersController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrdersController"/> class.
        /// </summary>
        /// <param name="orderFilter">The order filter service.</param>
        /// <param name="logger">The logger instance.</param>
        public OrdersController(IOrderHandler orderHandler, ILogger<OrdersController> logger)
        {
            _orderHandler = orderHandler;
            _logger = logger;
        }

        /// <summary>
        /// Filters orders based on the specified district and time range.
        /// </summary>
        /// <param name="district">The district to filter by.</param>
        /// <param name="from">The time at which filtering starts.</param>
        /// <param name="to">The time at which the filter ends.</param>
        /// <returns>A list of filtered orders, or 404 if no orders match the criteria.</returns>
        [HttpGet("filterFromTo")]
        public ActionResult<List<Order>> FilterOrdersFromTo([FromQuery] string district, [FromQuery] DateTimeOffset from, [FromQuery] DateTimeOffset to)
        {
            _logger.LogInformation("Received FilterOrdersFromTo request for district: {District} from: {From} to: {To} at {Time}", district, from, to, DateTime.Now);

            var filteredOrders = _orderHandler.FilterOrdersFromTo(district, from, to);

            if (!filteredOrders.Any())
            {
                _logger.LogWarning("No orders found for district: {District} from: {From} to: {To} at {Time}", district, from, to, DateTime.Now);
                return NotFound("No orders found matching the criteria.");
            }
            _logger.LogInformation("{Count} orders found for district: {District} from: {From} to: {To} at {LogTime}", filteredOrders.Count(), district, from, to, DateTime.Now);

            return Ok(filteredOrders);
        }

        /// <summary>
        /// Filters orders based on the specified district and target time.
        /// </summary>
        /// <param name="district">The district to filter by.</param>
        /// <param name="firstDeliverytime">The time of the first delivery.</param>
        /// <returns>A list of filtered orders, or 404 if no orders match the criteria.</returns>
        [HttpGet("filterTargetTime")]
        public ActionResult<IEnumerable<Order>> FilterOrdersTargetTime([FromQuery] string district, [FromQuery] DateTime firstDeliverytime)
        {
            _logger.LogInformation("Received FilterOrdersTargetTime request for district: {District} at time: {FirstDeliveryTime} at {Time}", district, firstDeliverytime, DateTime.Now);
            var filteredOrders = _orderHandler.FilterOrdersTargetTime(district, firstDeliverytime);
            if (!filteredOrders.Any())
            {
                _logger.LogWarning("No orders found for district: {District} at time: {FirstDeliveryTime} at {Time}", district, firstDeliverytime, DateTime.Now);
                return NotFound("No orders found matching the criteria.");
            }

            _logger.LogInformation("{Count} orders found for district: {District} at time: {FirstDeliveryTime} at {Time}", filteredOrders.Count(), district, firstDeliverytime, DateTime.Now);
            return Ok(filteredOrders);
        }
    }
}
