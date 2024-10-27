using EffectiveMobileTest1.Constants;
using EffectiveMobileTest1.Models;
using EffectiveMobileTest1.Services;
using Microsoft.AspNetCore.Mvc;

namespace EffectiveMobileTest1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderFilter _orderFilter;

        public OrdersController(IOrderFilter orderFilter)
        {
            _orderFilter = orderFilter;
        }

        [HttpGet("filterFromTo")]
        public ActionResult<List<Order>> FilterOrdersFromTo([FromQuery] string district, [FromQuery] DateTimeOffset from, [FromQuery] DateTimeOffset to)
        {
            var filteredOrders = _orderFilter.FilterOrdersFromTo(district, from, to);
            if (!filteredOrders.Any())
            {
                return NotFound("No orders found matching the criteria.");
            }
            return Ok(filteredOrders);
        }

        [HttpGet("filterTargetTime")]
        public ActionResult<IEnumerable<Order>> FilterOrdersTargetTime([FromQuery] string district, [FromQuery] DateTime firstDeliverytime)
        {
            var filteredOrders = _orderFilter.FilterOrdersTargetTime(district, firstDeliverytime);
            if (!filteredOrders.Any())
            {
                return NotFound("No orders found matching the criteria.");
            }
            return Ok(filteredOrders);
        }
    }
}
