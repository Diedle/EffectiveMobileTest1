using EffectiveMobileTest1.Constants;
using EffectiveMobileTest1.Models;
using Microsoft.Extensions.Caching.Memory;

namespace EffectiveMobileTest1.Services
{
    /// <summary>
    /// Handles order filtering and caching operations.
    /// </summary>
    public class OrderHandler : IOrderHandler
    {
        private readonly IFileManager _fileManager;
        private readonly IFilterOrdersService _filterOrdersService;
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _cache;
        private readonly ILogger<OrderHandler> _logger;
        private const string OrdersCacheKey = "OrdersCacheKey";

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderHandler"/> class.
        /// </summary>
        /// <param name="fileManager">The file manager for loading and saving orders.</param>
        /// <param name="filterOrdersService">The service for filtering orders.</param>
        /// <param name="configuration">The configuration settings.</param>
        /// <param name="cache">The memory cache.</param>
        /// <param name="logger">The logger instance.</param>
        public OrderHandler(IFileManager fileManager, IFilterOrdersService filterOrdersService, IConfiguration configuration,  IMemoryCache cache, ILogger<OrderHandler> logger)
        {
            _fileManager = fileManager;
            _filterOrdersService = filterOrdersService;
            _configuration = configuration;
            _cache = cache;
            _logger = logger;
        }

        /// <summary>
        /// Filters orders based on the specified district and time range.
        /// </summary>
        /// <param name="district">The district to filter by.</param>
        /// <param name="from">The time at which filtering starts.</param>
        /// <param name="to">The time at which the filter ends.</param>
        /// <returns>A list of filtered orders.</returns>
        public IEnumerable<Order> FilterOrdersFromTo(string district, DateTimeOffset from, DateTimeOffset to)
        {
            _logger.LogInformation("Starting FilterOrdersFromTo for district: {District} from: {From} to: {To} at {Time}", district, from, to, DateTime.Now);
            var orders = GetOrders();
            var result = _filterOrdersService.FilterOrders(orders, district, from, to);
            _logger.LogInformation("Completed FilterOrdersFromTo for district: {District} from: {From} to: {To} at {Time} with {OrderCount} orders found", district, from, to, DateTime.Now, result.Count());
            return result;
        }

        /// <summary>
        /// Filters orders based on the specified district and target time.
        /// </summary>
        /// <param name="district">The district to filter by.</param>
        /// <param name="firstDeliverytime">The time of the first delivery.</param>
        /// <returns>A list of filtered orders.</returns>
        public IEnumerable<Order> FilterOrdersTargetTime(string district, DateTime firstDeliverytime)
        {
            _logger.LogInformation("Starting FilterOrdersTargetTime for district: {District} at time: {FirstDeliveryTime} at {Time}", district, firstDeliverytime, DateTime.Now);

            var orders = GetOrders();
            DateTime targetTime = firstDeliverytime.AddMinutes(double.Parse(_configuration[VariableContants.TimeRangeMinutes]));

            _logger.LogInformation("Filtering orders for district: {District} between {FirstDeliveryTime} and {TargetTime} at {Time}", district, firstDeliverytime, targetTime, DateTime.Now);
            var filteredOrders = _filterOrdersService.FilterOrders(orders, district, firstDeliverytime, targetTime);

            _logger.LogInformation("{Count} orders filtered for district: {District} from: {FirstDeliveryTime} to: {TargetTime} at {Time}", filteredOrders.Count, district, firstDeliverytime, targetTime, DateTime.Now);
            _fileManager.SaveFilteredOrdersToFile(filteredOrders, _configuration[VariableContants.OutputFilePath]);

            _logger.LogInformation("Filtered orders saved to file: {FilePath} at {Time}", _configuration[VariableContants.OutputFilePath], DateTime.Now);
            return filteredOrders;
        }

        /// <summary>
        /// Retrieves orders from cache or loads them from file if not cached.
        /// </summary>
        /// <returns>A list of orders.</returns>
        private List<Order> GetOrders()
        {
            _logger.LogInformation("Attempting to retrieve orders from cache at {Time}", DateTime.Now);
            if (!_cache.TryGetValue(OrdersCacheKey, out List<Order>? orders))
            {
                _logger.LogInformation("Cache miss. Loading orders from file at {Time}", DateTime.Now);

                var filePath = _configuration[VariableContants.InputFilePath];
                orders = _fileManager.LoadOrdersFromFile(filePath);
                if (orders != null && orders.Count > 0)
                {
                    _logger.LogInformation("Successfully loaded {OrderCount} orders from file at {Time}", orders.Count, DateTime.Now);
                    _cache.Set(OrdersCacheKey, orders, TimeSpan.FromMinutes(double.Parse(_configuration[VariableContants.CacheLifeTime])));
                    _logger.LogInformation("Orders cached at {Time}", DateTime.Now);
                }
            }
            else
            {
                _logger.LogInformation("Orders retrieved from cache at {Time}", DateTime.Now);
            }
            return orders ?? [];
        }
    }
}
