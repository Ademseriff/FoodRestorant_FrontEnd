using MassTransit;
using Shared.Enums;
using Shared.Events;
using Shared.Message;
using System.Net.Http;
using System.Text;

namespace FrontEndHomePage.Consumers
{
    public class BasketViewResponseEventConsumer : IConsumer<BasketViewResponseEvent>
    {
       
        public static BasketViewResponseEvent OrderItemMessega = new BasketViewResponseEvent();

       public static int listLength = 0;
        public async Task Consume(ConsumeContext<BasketViewResponseEvent> context)
        {


            OrderItemMessega.OrderItems = context.Message.OrderItems;

          

        }
    }
}
