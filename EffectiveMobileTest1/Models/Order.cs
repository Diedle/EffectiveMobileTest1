namespace EffectiveMobileTest1.Models
{
    public class Order
    {
        /// <summary>
        /// Gets or sets the unique identifier for the order.
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// Gets or sets the weight of the order in kilograms.
        /// </summary>
        public double Weight { get; set; }

        /// <summary>
        /// Gets or sets the district where the order is to be delivered.
        /// </summary>
        public string District { get; set; }

        /// <summary>
        /// Gets or sets the delivery time of the order.
        /// </summary> 
        public DateTimeOffset DeliveryTime { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Order"/> class.
        /// </summary>
        /// <param name="orderId">The unique identifier for the order.</param>
        /// <param name="weight">The weight of the order in kilograms.</param>
        /// <param name="district">The district where the order is to be delivered.</param>
        /// <param name="deliveryTime">The delivery time of the order.</param>
        public Order(string orderId, double weight, string district, DateTimeOffset deliveryTime)
        {
            OrderId = orderId;
            Weight = weight;
            District = district;
            DeliveryTime = deliveryTime;
        }
    }
}
