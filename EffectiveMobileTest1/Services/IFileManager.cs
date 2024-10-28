using EffectiveMobileTest1.Models;

namespace EffectiveMobileTest1.Services
{
    /// <summary>
    /// Defines methods for managing the loading and saving of orders to and from files.
    /// </summary>
    public interface IFileManager
    {
        /// <summary>
        /// Loads orders from the specified file.
        /// </summary>
        /// <param name="filePath">The path to the file containing the orders.</param>
        /// <returns>A list of orders.</returns>
        List<Order> LoadOrdersFromFile(string filePath);

        /// <summary>
        /// Saves filtered orders to the specified file.
        /// </summary>
        /// <param name="orders">The list of orders to save.</param>
        /// <param name="filePath">The path to the file where the orders will be saved.</param>
        void SaveFilteredOrdersToFile(List<Order> orders, string filePath);
    }
}
