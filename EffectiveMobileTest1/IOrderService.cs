namespace EffectiveMobileTest1
{
    public interface IOrderService
    {
        List<Order> LoadOrdersFromFile(string filePath);
        List<Order> FilterOrders(List<Order> orders, string district, DateTimeOffset from, DateTimeOffset to);
    }
}
