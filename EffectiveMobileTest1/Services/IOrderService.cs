using EffectiveMobileTest1.Models;

namespace EffectiveMobileTest1.Services
{
    public interface IOrderService
    {
        List<Order> FilterOrders(List<Order> orders, string district, DateTimeOffset from, DateTimeOffset to);
    }
}
