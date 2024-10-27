using EffectiveMobileTest1.Models;

namespace EffectiveMobileTest1.Services
{
    public class OrderService : IOrderService
    {
        public List<Order> FilterOrders(List<Order> orders, string district, DateTimeOffset from, DateTimeOffset to)
        {
            return orders.Where(order =>
                order.District.Equals(district, StringComparison.OrdinalIgnoreCase) &&
                order.DeliveryTime >= from && order.DeliveryTime <= to).ToList();
        }
    }
}
