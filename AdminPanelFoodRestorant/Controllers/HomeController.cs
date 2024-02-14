using AdminPanelFoodRestorant.Consumers;
using AdminPanelFoodRestorant.Models;
using AdminPanelFoodRestorant.ViewModel;
using MassTransit;
using MassTransit.Testing;
using Microsoft.AspNetCore.Mvc;
using Shared.Enums;
using Shared.Events;
using System.Diagnostics;

namespace AdminPanelFoodRestorant.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPublishEndpoint publishEndpoint;

        public HomeController(ILogger<HomeController> logger, IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            this.publishEndpoint = publishEndpoint;
            //this.publishedMessage = publishedMessage;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> OrderView()
        {
           List<OrdersContentsVM> ordersContentsVM = new List<OrdersContentsVM>();
            OrderViewRequestEvent orderViewRequestEvent = new OrderViewRequestEvent();
            await  publishEndpoint.Publish(orderViewRequestEvent);

            await Task.Delay(7000);

            if(OrderViewResponseEventConsumer.orderViewResponseEvent.OrderViewResponseEventMessage == null)
            {
                ordersContentsVM.Add(new OrdersContentsVM()
                {
                    Category = (Category)0,
                    Email ="YOK",
                    TotalPrice ="YOK",
                    OrderId =0,
                    Product ="YOK"
                });
            }
            else
            {
                foreach (var x in OrderViewResponseEventConsumer.orderViewResponseEvent.OrderViewResponseEventMessage)
                {
                    ordersContentsVM.Add(new OrdersContentsVM()
                    {
                        Category = x.Category,
                        Email = x.Email,
                        OrderId = x.OrderId,
                        Product = x.Product,
                        TotalPrice = x.TotalPrice,
                        Adress = x.Adress,
                        PhoneNumber = x.PhoneNumber
                    });
                }
            }
            
            return View(ordersContentsVM);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
