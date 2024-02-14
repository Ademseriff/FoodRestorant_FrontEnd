using MassTransit;
using Shared.Events;

namespace AdminPanelFoodRestorant.Consumers
{
    public class OrderViewResponseEventConsumer : IConsumer<OrderViewResponseEvent>
    {
        public static OrderViewResponseEvent orderViewResponseEvent = new OrderViewResponseEvent();
        public async Task Consume(ConsumeContext<OrderViewResponseEvent> context)
        {
            orderViewResponseEvent.OrderViewResponseEventMessage = context.Message.OrderViewResponseEventMessage;
        }
    }
}
