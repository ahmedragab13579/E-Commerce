using System.Text.Json.Serialization;

namespace SharedMessages
{
    public class OrderPlacedEvent
    {
        public int OrderId { get;private set; }
        public string UserName { get; private set; }
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
        public decimal TotalAmount { get; private set; }
        public DateTime OrderDate { get; private set; }
        public string UserEmail { get; private set; } 



        private OrderPlacedEvent()
        {
            
        }


        public OrderPlacedEvent(int orderId, string userName, decimal totalAmount, DateTime orderDate, string userEmail )
        {
            if (orderId <= 0)
                throw new ArgumentException("OrderId must be greater than zero.", nameof(orderId));
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentException("UserName cannot be null or empty.", nameof(userName));
            if (totalAmount < 0)
                throw new ArgumentException("TotalAmount cannot be negative.", nameof(totalAmount));
            if (orderDate < DateTime.MinValue)
                throw new ArgumentException("OrderDate is not valid.", nameof(orderDate));
            if (orderDate > DateTime.UtcNow.AddMinutes(1))
                throw new ArgumentException("OrderDate cannot be in the future.", nameof(orderDate));
            if (string.IsNullOrEmpty(userEmail) || !userEmail.Contains("@"))
                throw new ArgumentException("UserEmail is not valid.", nameof(userEmail));
            OrderId = orderId;
            UserName = userName;
            TotalAmount = totalAmount;
            OrderDate = orderDate;
            UserEmail = userEmail;
        }
    }
}
