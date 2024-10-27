using EffectiveMobileTest1.Models;

namespace EffectiveMobileTest1.Services
{
    public interface IOrderFilter
    {
        IEnumerable<Order> FilterOrdersFromTo(string district, DateTimeOffset from, DateTimeOffset to);

        IEnumerable<Order> FilterOrdersTargetTime(string district, DateTime firstDeliverytime);
    }
}
