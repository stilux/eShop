using System;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Courier;
using Microsoft.Extensions.Logging;
using Shared.Contracts.Helpers;
using Shared.Contracts.Messages;
using Shared.Contracts.Requests;

namespace OrderService.Courier.Activities
{
    public class ReserveProductsActivity : IActivity<IReserveProducts, IReserveProductsLog>
    {
        private readonly IRequestClient<IReserveProducts> _requestClient;
        private readonly ILogger<ReserveProductsActivity> _logger;
        private static readonly Uri ReserveProductsMessageUri = QueueNames.GetMessageUri(nameof(IReserveProducts));
        private static readonly Uri CancelReservationMessageUri = QueueNames.GetMessageUri(nameof(ICancelReservation));

        public ReserveProductsActivity(IRequestClient<IReserveProducts> requestClient,
            ILogger<ReserveProductsActivity> logger)
        {
            _requestClient = requestClient;
            _logger = logger;
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<IReserveProducts> context)
        {
            _logger.LogInformation($"Reserve Products called for order {context.Arguments.OrderId}");

            // var sendEndpoint = await context.GetSendEndpoint(ReserveProductsMessageUri);
            // await sendEndpoint.Send<IReserveProducts>(new
            // {
            //     CorrelationId = context.Arguments.CorrelationId,
            //     OrderId = context.Arguments.OrderId,
            //     Items = context.Arguments.Items
            // });

            await _requestClient.GetResponse<IReserveProductsResult>(new
            {
                CorrelationId = context.Arguments.CorrelationId,
                OrderId = context.Arguments.OrderId,
                Items = context.Arguments.Items
            });
            
            return context.Completed(new {context.Arguments.CorrelationId, context.Arguments.OrderId});
        }

        public async Task<CompensationResult> Compensate(CompensateContext<IReserveProductsLog> context)
        {
            _logger.LogInformation($"Compensate Reserve Products called for order {context.Log.OrderId}");

            var sendEndpoint = await context.GetSendEndpoint(CancelReservationMessageUri);
            await sendEndpoint.Send<ICancelReservation>(new
            {
                CorrelationId = context.Log.CorrelationId,
                OrderId = context.Log.OrderId
            });
            return context.Compensated();
        }
    }

    public interface IReserveProductsLog
    {
        Guid CorrelationId { get; }
        int OrderId { get; }
    }
}