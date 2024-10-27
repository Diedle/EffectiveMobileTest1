using EffectiveMobileTest1.Models;

namespace EffectiveMobileTest1.Services
{
    public interface IFileManager
    {
       
        List<Order> LoadOrdersFromFile(string filePath);

        void SaveFilteredOrdersToFile(List<Order> orders, string filePath);
    }
}
