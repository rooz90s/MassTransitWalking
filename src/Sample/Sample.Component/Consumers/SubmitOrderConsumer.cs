using MassTransit;
using Microsoft.Extensions.Logging;
using Sample.Contract;
using Sample.Contract.SagaEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Component.Consumers
{
    public class SubmitOrderConsumer : IConsumer<ISubmitOrder>
    {

        private readonly ILogger<SubmitOrderConsumer> _logger;

        public SubmitOrderConsumer(ILogger<SubmitOrderConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ISubmitOrder> context)
        {

            _logger.Log(LogLevel.Information, $"Consumer with OrderId : {context.Message.OrderId}");

            await context.RespondAsync<IOrderSubmissionAccepterResponse>(new
            {
                OrderId = context.Message.OrderId,
                Timestamp = InVar.Timestamp,
                CustomerNumber = context.Message.CustomerNumber
            });

            await context.Publish<IOrderSubmitted>(new
            {
                OrderId = context.Message.OrderId,
                Timestamp = InVar.Timestamp,
                CustomerNumber = context.Message.CustomerNumber
            });

            
        }
    }
}
