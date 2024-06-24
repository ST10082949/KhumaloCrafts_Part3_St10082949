using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Khumalo_durableFunction
{
    public static class OrderHttpStarter
    {
        [Function("StartOrderOrchestration")]
        public static async Task<HttpResponseData> StartOrderOrchestration(
            [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req,
            [DurableClient] DurableTaskClient starter,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("OrderHttpStarter");

            var requestBody = await JsonSerializer.DeserializeAsync<Order>(req.Body);

            string instanceId = await starter.ScheduleNewOrchestrationInstanceAsync(nameof(OrderOrchestrator), requestBody);

            logger.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            var response = req.CreateResponse(HttpStatusCode.Accepted);
            response.Headers.Add("Content-Type", "application/json");
            response.WriteString(JsonSerializer.Serialize(new { instanceId }));

            return response;
        }
    }

}
