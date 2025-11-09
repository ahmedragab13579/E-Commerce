using MassTransit;
using NotificationService.Service.Interfaces;
using SharedMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Consumers
{
    public class Consumer : IConsumer<OrderPlacedEvent>
    {
        private readonly ILogger<Consumer> _logger;
        private readonly IEmailService _emailService; 

        public Consumer(ILogger<Consumer> logger, IEmailService emailService)
        {
            _logger = logger;
            _emailService = emailService; 
        }

        public async Task Consume(ConsumeContext<OrderPlacedEvent> context)
        {
            var message = context.Message; 

            _logger.LogInformation($"===== NEW ORDER RECEIVED ======");
            _logger.LogInformation($"Order ID: {message.OrderId}, User: {message.UserName}");

            string subject = $"Your order {message.OrderId} is confirmed!";
            string body = $"Hi {message.UserName},<br>Thank you for your order! Your total amount is {message.TotalAmount}.";

           
            await _emailService.SendEmailAsync(
                message.UserEmail, 
                subject,
                body
            );

            _logger.LogInformation($"Email sent for Order {message.OrderId}.");
            _logger.LogInformation($"===============================");
        }
    }
}
