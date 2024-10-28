using EffectiveMobileTest1.Models;

namespace EffectiveMobileTest1.Services
{
    /// <summary>
    /// Provides filtering of processing orders and caching operations.
    /// </summary>
    public interface IOrderHandler
    {
        /// <summary>
        /// Filters orders based on the specified district and time range.
        /// </summary>
        /// <param name="district">The district to filter by.</param>
        /// <param name="from">The time at which filtering starts.</param>
        /// <param name="to">The time at which the filter ends.</param>
        /// <returns>A list of filtered orders.</returns>
        IEnumerable<Order> FilterOrdersFromTo(string district, DateTimeOffset from, DateTimeOffset to);

        /// <summary>
        /// Filters orders based on the specified district and target time.
        /// </summary>
        /// <param name="district">The district to filter by.</param>
        /// <param name="firstDeliverytime">The time of the first delivery.</param>
        /// <returns>A list of filtered orders.</returns>
        IEnumerable<Order> FilterOrdersTargetTime(string district, DateTime firstDeliverytime);
    }
}
