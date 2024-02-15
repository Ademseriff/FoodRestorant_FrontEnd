using MassTransit;
using Shared.Events;

namespace AdminPanelFoodRestorant.Consumers
{
    public class OrderViewComplatedEventResponseConsumer : IConsumer<OrderViewComplatedEventResponse>
    {
        public static OrderViewComplatedEventResponse orderViewResponseEvent = new OrderViewComplatedEventResponse();
       
        public async Task Consume(ConsumeContext<OrderViewComplatedEventResponse> context)
        {
            orderViewResponseEvent.orderViewComplatedEventResponseMessages = context.Message.orderViewComplatedEventResponseMessages;
        }
    }
}
