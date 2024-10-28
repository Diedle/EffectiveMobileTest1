using EffectiveMobileTest1.Models;
using Newtonsoft.Json;

namespace EffectiveMobileTest1.Services
{
    /// <summary>
    /// Working with files.
    /// </summary>
    public class FileManager : IFileManager
    {
        // <summary>
        /// Loads orders from the specified file.
        /// </summary>
        /// <param name="filePath">The path to the file containing the orders.</param>
        /// <returns>A list of orders.</returns>
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

        /// <summary>
        /// Saves filtered orders to the specified file.
        /// </summary>
        /// <param name="orders">The list of orders to save.</param>
        /// <param name="filePath">The path to the file where the orders will be saved.</param>
        public void SaveFilteredOrdersToFile(List<Order> orders, string filePath)
        {
            using (var streamWriter = new StreamWriter(filePath))
            {
                var serializer = new JsonSerializer();
                foreach (var order in orders)
                {
                    serializer.Serialize(streamWriter, order);
                    streamWriter.WriteLine();
                }
            }
        }
    }
}
