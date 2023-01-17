using MassTransit;
using Sample.Component.StateMachines.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Component.StateMachines
{
    public class OrderSateMachineDefinition : SagaDefinition<OrderState>
    {
        protected override void ConfigureSaga(IReceiveEndpointConfigurator endpointConfigurator, ISagaConfigurator<OrderState> sagaConfigurator)
        {
            endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 5000, 10000));
            endpointConfigurator.UseInMemoryOutbox();
        }
    }
}
