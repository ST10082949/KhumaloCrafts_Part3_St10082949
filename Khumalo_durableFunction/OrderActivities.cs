using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Khumalo_durableFunction
{
    public static class OrderActivities
    {
        [Function(nameof(PlaceOrder))]
        public static async Task PlaceOrder([Microsoft.Azure.Functions.Worker.ActivityTrigger] Order order, FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger(nameof(PlaceOrder));
            // Logic to place the order
            logger.LogInformation($"Order {order.Id} has been placed.");
            await Task.CompletedTask;
        }

        [Function(nameof(ChangeQuantity))]
        public static async Task ChangeQuantity([Microsoft.Azure.Functions.Worker.ActivityTrigger] Order order, FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger(nameof(ChangeQuantity));
            // Logic to change the order quantity
            logger.LogInformation($"Order {order.Id} quantity changed to {order.Quantity}.");
            await Task.CompletedTask;
        }

        [Function(nameof(UpdateOrderHistory))]
        public static async Task UpdateOrderHistory([Microsoft.Azure.Functions.Worker.ActivityTrigger] Order order, FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger(nameof(UpdateOrderHistory));
            // Logic to update order history
            logger.LogInformation($"Order {order.Id} history updated.");
            await Task.CompletedTask;
        }

        [Function(nameof(SendNotification))]
        public static async Task SendNotification([Microsoft.Azure.Functions.Worker.ActivityTrigger] Order order, FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger(nameof(SendNotification));
            string topicEndpoint = Environment.GetEnvironmentVariable("EventGridTopicEndpoint");
            string topicKey = Environment.GetEnvironmentVariable("EventGridTopicKey");

            var topicHostname = new Uri(topicEndpoint).Host;
            var credentials = new TopicCredentials(topicKey);
            var client = new EventGridClient(credentials);

            var eventGridEvent = new EventGridEvent
            {
                Id = Guid.NewGuid().ToString(),
                EventType = "OrderNotification",
                Data = new
                {
                    OrderId = order.Id,
                    Quantity = order.Quantity,
                    Email = order.CustomerEmail
                },
                EventTime = DateTime.UtcNow,
                Subject = $"Order/{order.Id}",
                DataVersion = "1.0"
            };

            await client.PublishEventsAsync(topicHostname, new List<EventGridEvent> { eventGridEvent });
            logger.LogInformation($"Notification sent for Order {order.Id}.");
        }
    }
}
