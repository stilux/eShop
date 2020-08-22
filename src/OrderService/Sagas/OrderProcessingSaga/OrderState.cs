using System;
using Automatonymous;

namespace OrderService.Sagas.OrderProcessingSaga
{
    public class OrderState : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public State CurrentState { get; set; }
        public int OrderId { get; set; }
    }
}