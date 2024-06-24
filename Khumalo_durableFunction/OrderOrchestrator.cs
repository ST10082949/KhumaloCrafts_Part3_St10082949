using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Khumalo_durableFunction
{
    
   
        public static class OrderOrchestrator
        {
            [Function(nameof(OrderOrchestrator))]
            public static async Task RunOrchestrator([OrchestrationTrigger] TaskOrchestrationContext context)
            {
                var order = context.GetInput<Order>();

                var logger = context.CreateReplaySafeLogger(nameof(OrderOrchestrator));
                logger.LogInformation($"Starting order processing for Order ID: {order.Id}");

                await context.CallActivityAsync(nameof(OrderActivities.PlaceOrder), order);
                await context.CallActivityAsync(nameof(OrderActivities.ChangeQuantity), order);
                await context.CallActivityAsync(nameof(OrderActivities.UpdateOrderHistory), order);
                await context.CallActivityAsync(nameof(OrderActivities.SendNotification), order);

                logger.LogInformation($"Completed order processing for Order ID: {order.Id}");
            }
        }


    

}
