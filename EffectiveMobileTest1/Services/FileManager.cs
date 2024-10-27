using EffectiveMobileTest1.Models;
using Newtonsoft.Json;

namespace EffectiveMobileTest1.Services
{
    public class FileManager : IFileManager
    {
        public List<Order> LoadOrdersFromFile(string filePath)
        {
            var orders = new List<Order>();

            using (var fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read))
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

        public void SaveFilteredOrdersToFile(List<Order> orders, string filePath)
        {
            using (var streamWriter = new StreamWriter(filePath))
            {
                var serializer = new JsonSerializer();
                foreach (var order in orders)
                {
                    serializer.Serialize(streamWriter, order);
                    streamWriter.WriteLine();  // Записываем перевод строки после каждой записи
                }
            }
        }
    }
}
