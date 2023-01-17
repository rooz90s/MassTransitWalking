using MassTransit;
using Sample.Component.StateMachines.StateMachineActivities;
using Sample.Component.StateMachines.States;
using Sample.Contract;
using Sample.Contract.SagaEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Component.StateMachines
{
    public class OrderStateMachine : MassTransitStateMachine<OrderState>
    {

        public OrderStateMachine()
        {


            Event(() => OrderSubmitted, x => x.CorrelateById(m => m.Message.OrderId));
            Event(() => CheckOrder, x => x.CorrelateBy((saga, ctx) => saga.CorrelationId == ctx.Message.OrderId));
            Event(() => AccountClosed, x => x.CorrelateBy((saga, ctx) => saga.CustomerNumbr == ctx.Message.CustomerNumber));
            Event(() => OrderAccepted, x => x.CorrelateBy((saga, ctx) => saga.CorrelationId == ctx.Message.OrderId));


            InstanceState(x => x.CurrentState);

            Initially(
                When(OrderSubmitted)
                .Then(ctx =>
                {

                    ctx.Instance.OrderId = ctx.Message.OrderId;
                    ctx.Instance.CustomerNumbr = ctx.Message.CustomerNumber;
                    ctx.Instance.Updated = InVar.Timestamp;
                })
                .TransitionTo(Submitted));

            During(Submitted,
                Ignore(OrderSubmitted),
                When(AccountClosed)
                .TransitionTo(Canceled),
                When(OrderAccepted)
                .Activity(x=>x.OfType<AcceptOrderActivity>()));


            DuringAny(
                When(CheckOrder)
                .RespondAsync(x=> x.Init<IOrderStatus>(new
                {
                    OrderId = x.Instance.OrderId,
                    CustomerId = x.Instance.CustomerNumbr,
                    Updated = x.Instance.Updated,
                    Status = x.Instance.CurrentState
                }))
                );

        }


        public State Submitted { get; set; }
        public State Canceled { get; set; }
        public State Accepted { get; set; }


        public Event<IOrderSubmitted> OrderSubmitted { get; set; }
        public Event<IOrderCheck> CheckOrder { get; set; }
        public Event<ICustomerAccountClosed> AccountClosed { get; set; }
        public Event<IOrderAccepted> OrderAccepted{ get; set; }

    }
}
