using EffectiveMobileTest1.Models;

namespace EffectiveMobileTest1.Services
{
    /// <summary>
    /// Provides services for filtering orders based on specified criteria.
    /// </summary>
    public class FilterOrdersService : IFilterOrdersService
    {
        /// <summary>
        /// Filters orders based on the specified district and time range.
        /// </summary>
        /// <param name="orders">The list of orders to filter.</param>
        /// <param name="district">The district to filter by.</param>
        /// <param name="from">The start time of the delivery range.</param>
        /// <param name="to">The end time of the delivery range.</param>
        /// <returns>A list of filtered orders.</returns>
        public List<Order> FilterOrders(List<Order> orders, string district, DateTimeOffset from, DateTimeOffset to)
        {
            return orders.Where(order =>
                order.District.Equals(district, StringComparison.OrdinalIgnoreCase) &&
                order.DeliveryTime >= from && order.DeliveryTime <= to).ToList();
        }
    }
}
