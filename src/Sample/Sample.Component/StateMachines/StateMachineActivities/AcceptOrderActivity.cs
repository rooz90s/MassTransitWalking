using Automatonymous;
using MassTransit;
using Microsoft.Extensions.Logging;
using Sample.Component.StateMachines.States;
using Sample.Contract.SagaEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Component.StateMachines.StateMachineActivities
{
    public class AcceptOrderActivity : IStateMachineActivity<OrderState, IOrderAccepted>
    {

        private readonly ILogger<AcceptOrderActivity> _logger;

        public AcceptOrderActivity(ILogger<AcceptOrderActivity> logger)
        {
            _logger = logger;
        }

        public void Accept(StateMachineVisitor visitor)
        {

            _logger.LogInformation("Accept from activity");
            visitor.Visit(this);
        }

        public async Task Execute(BehaviorContext<OrderState, IOrderAccepted> context, IBehavior<OrderState, IOrderAccepted> next)
        {
            _logger.LogInformation($"Execute from activity orderID : {context.Data.OrderId}");
            await next.Execute(context).ConfigureAwait(false);
        }

        public Task Faulted<TException>(BehaviorExceptionContext<OrderState, IOrderAccepted, TException> context, IBehavior<OrderState, IOrderAccepted> next) where TException : Exception
        {
            _logger.LogInformation("Faulted from activity");
            return next.Faulted(context);
        }

        public void Probe(ProbeContext context)
        {
            _logger.LogInformation("Probe from activity");
            var scope = context.CreateScope("accept-order");
            
        }
    }
}
