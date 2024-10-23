using System.Globalization;
using Newtonsoft.Json;


namespace EffectiveMobileTest1
{
    public static class OrderService
    {
        public static List<Order> LoadOrdersFromFile(string filePath)
        {
            var orders = new List<Order>();

            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            using (var streamReader = new StreamReader(fileStream))
            using (var jsonReader = new JsonTextReader(streamReader))
            {
                var serializer = new JsonSerializer();
                jsonReader.SupportMultipleContent = true;

                while (jsonReader.Read())
                {
                    if (jsonReader.TokenType == JsonToken.StartObject)
                    {
                        var order = serializer.Deserialize<Order>(jsonReader);
                        if (order != null)
                        {
                            orders.Add(order);
                        }
                    }
                }
            }

            return orders;
        }

        public static List<Order> FilterOrders(List<Order> orders, string district, DateTimeOffset from, DateTimeOffset to)
        {
            return orders.Where(order =>
                order.District.Equals(district, StringComparison.OrdinalIgnoreCase) &&
                order.DeliveryTime >= from && order.DeliveryTime <= to).ToList();
        }
    }
}
