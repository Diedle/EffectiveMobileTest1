using EffectiveMobileTest1.Constants;
using EffectiveMobileTest1.Models;
using Microsoft.Extensions.Caching.Memory;

namespace EffectiveMobileTest1.Services
{
    public class OrderFilter : IOrderFilter
    {
        private readonly IFileManager _fileManager;
        private readonly IOrderService _orderService;
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _cache;
        private const string OrdersCacheKey = "OrdersCacheKey";

        public OrderFilter(IFileManager fileManager, IOrderService orderService, IConfiguration configuration,  IMemoryCache cache)
        {
            _fileManager = fileManager;
            _orderService = orderService;
            _configuration = configuration;
            _cache = cache;
        }

        public IEnumerable<Order> FilterOrdersFromTo(string district, DateTimeOffset from, DateTimeOffset to)
        {
            var orders = GetOrders();
            return _orderService.FilterOrders(orders, district, from, to);
        }

        public IEnumerable<Order> FilterOrdersTargetTime(string district, DateTime firstDeliverytime)
        {
            var orders = GetOrders();
            DateTime targetTime = firstDeliverytime.AddMinutes(double.Parse(_configuration[VariableContants.TimeRangeMinutes]));
            var filteredOrders = _orderService.FilterOrders(orders, district, firstDeliverytime, targetTime);

            _fileManager.SaveFilteredOrdersToFile(filteredOrders, _configuration[VariableContants.OutputFilePath]);
            return filteredOrders;
        }

        private List<Order> GetOrders()
        {
            if (!_cache.TryGetValue(OrdersCacheKey, out List<Order> orders))
            {
                var filePath = _configuration[VariableContants.InputFilePath];
                orders = _fileManager.LoadOrdersFromFile(filePath);
                _cache.Set(OrdersCacheKey, orders); 
            }
            return orders;
        }
    }
}
