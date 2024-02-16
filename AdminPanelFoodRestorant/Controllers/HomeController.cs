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


        public async Task<IActionResult> OrderComplated(int OrderId,string Email)
        {
            OrderComplatedEvent orderComplatedEvent = new OrderComplatedEvent();
            //mail işlemleri
            MailSentEvent mailSentEvent = new MailSentEvent();
            mailSentEvent.State = Shared.Enums.State.successful;
            mailSentEvent.OrderId = OrderId;
            mailSentEvent.EMail = Email;
            await publishEndpoint.Publish(mailSentEvent);
            //--

            orderComplatedEvent.id = OrderId;
            await publishEndpoint.Publish(orderComplatedEvent);
            return RedirectToAction(actionName: "OrderView", controllerName: "Home");
        }

        public async Task<IActionResult> OrderFailed(int OrderId,string Email)
        {
            OrderFailedEvent orderFailedEvent = new OrderFailedEvent();
            //mail işlemleri
            MailSentEvent mailSentEvent = new MailSentEvent();
            mailSentEvent.State = Shared.Enums.State.failed;
            mailSentEvent.OrderId = OrderId;
            mailSentEvent.EMail = Email;
            await publishEndpoint.Publish(mailSentEvent);
            //--

            orderFailedEvent.id = OrderId;
            await publishEndpoint.Publish(orderFailedEvent);
            return RedirectToAction(actionName: "OrderView", controllerName: "Home");
        }


        public async Task<IActionResult> OrderViewCompLated()
        {
            List<OrdersContentsVM> ordersContentsVM = new List<OrdersContentsVM>();
            OrderViewCompLatedEvent orderViewCompLatedEvent = new OrderViewCompLatedEvent();
            await publishEndpoint.Publish(orderViewCompLatedEvent);

            await Task.Delay(6000);
            int toplam=0, kontrolid = 0;
            if (OrderViewComplatedEventResponseConsumer.orderViewResponseEvent.orderViewComplatedEventResponseMessages == null)
            {
                ordersContentsVM.Add(new OrdersContentsVM()
                {
                    Category = (Category)0,
                    Email = "YOK",
                    TotalPrice = "YOK",
                    OrderId = 0,
                    Product = "YOK"
                });
            }
            else
            {
                foreach (var x in OrderViewComplatedEventResponseConsumer.orderViewResponseEvent.orderViewComplatedEventResponseMessages)
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
                    if(kontrolid != x.OrderId)
                    {
                        toplam += int.Parse(x.TotalPrice);
                        kontrolid = x.OrderId;
                    }
                }
            }
            ViewBag.TotalPrice = toplam;
            return View(ordersContentsVM);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
