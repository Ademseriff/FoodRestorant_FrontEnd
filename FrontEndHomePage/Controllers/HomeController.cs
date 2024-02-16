using FrontEndHomePage.Consumers;
using FrontEndHomePage.Models;
using FrontEndHomePage.ViewModel;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using Shared.Enums;
using Shared.Events;
using Shared.Message;
using System.Diagnostics;

namespace FrontEndHomePage.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPublishEndpoint publishEndpoint;
       

        public HomeController(ILogger<HomeController> logger, IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            this.publishEndpoint = publishEndpoint;
         
        }

        public IActionResult Index()
        {
            return View();
        }



        public IActionResult Menu()
        {
            return View();
        }


        public IActionResult About()
        {
            return View();
        }

        public async Task<IActionResult> Sepet()
        {

          
            BasketViewRepuestEvent repuestEvent = new BasketViewRepuestEvent();
            await publishEndpoint.Publish(repuestEvent);
            await Task.Delay(TimeSpan.FromSeconds(8));
            int toplam = 0;
            List<BasketVM> basketVM  = new List<BasketVM>();
            //sepet boş mu dolu mu kontorollü
            if(BasketViewResponseEventConsumer.OrderItemMessega.OrderItems != null)
            {
                foreach (var vm in BasketViewResponseEventConsumer.OrderItemMessega.OrderItems)
                {
                    basketVM.Add(new BasketVM
                    {
                        Id = vm.Id,
                        Price = vm.Price,
                        Product = vm.Product,
                        Category = vm.Category,
                        Adress ="belirtilmedi",
                        PhoneNumber = "belirtilmedi"
                    });
                    toplam = toplam + Int32.Parse(vm.Price);
                }
                foreach (var vm in basketVM)
                {
                    vm.TotalPrice = toplam.ToString();
                }
            }
            else
            {
                basketVM.Add(new BasketVM
                {
                    Id = 0,
                    TotalPrice="0",
                    Category = 0,
                    Price = "0",
                    Product ="0",
                    Adress = "belirtilmedi",
                    PhoneNumber = "belirtilmedi"
                });
            }
        

            return View(basketVM);
        }


        [HttpPost]
        public async Task<IActionResult> Sepet(List<BasketVM> basketVM ,string PhoneNumber, string Adress,string EMail)
        {
          
            foreach (var i  in basketVM)
            {
                i.Adress = Adress;
                i.PhoneNumber = PhoneNumber;
                i.EMail = EMail;
            }
            //sipariş oluşturma işlemleri ve ardından kuyruga publis işlemi yapılacak.
            //sipariş id random vericem o yüzden burda random bir sayı oluşturuyprum.
            Random random = new Random();
            int rastgeleSayi = random.Next(1, 9999);

            OrderCreatedEvent orderCreatedEvent = new OrderCreatedEvent();
            orderCreatedEvent.Id = rastgeleSayi;
            orderCreatedEvent.State = Shared.Enums.State.pending;

            //mail işlemleri 
            MailSentEvent mailSentEvent = new MailSentEvent();
            mailSentEvent.EMail = EMail;
            mailSentEvent.State = Shared.Enums.State.pending;
            mailSentEvent.OrderId = rastgeleSayi;
            await publishEndpoint.Publish(mailSentEvent);
            //mail işleri bittiş

            orderCreatedEvent.OrderCreatedEventMessage = basketVM.Select(oi => new OrderCreatedEventMessage()
            {
                Category = oi.Category,
                Product = oi.Product,
                TotalPrice = oi.TotalPrice,
                Email = oi.EMail,
                Adress =oi.Adress,
                PhoneNumber = oi.PhoneNumber,
            }).ToList();
            await publishEndpoint.Publish(orderCreatedEvent);


            //sepet kısmını temizleme işlemleri.
            BasketDeleteRequestEvent basketDeleteRequestEvent = new BasketDeleteRequestEvent();
            await publishEndpoint.Publish(basketDeleteRequestEvent);

            return RedirectToAction(actionName: "Index", controllerName: "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Clear()
        {


            //sepet kısmını temizleme işlemleri.
            BasketDeleteRequestEvent basketDeleteRequestEvent = new BasketDeleteRequestEvent();
            await publishEndpoint.Publish(basketDeleteRequestEvent);

            return RedirectToAction(actionName: "Index", controllerName: "Home");
        }
        public async Task<IActionResult> SepetProcess(int id)
        {
            int Catagory0, Price0,Product0;
            Catagory0 = id % 10;
            id = id / 10;
            Product0 = id % 10;
            id = id / 10;
            Price0 = id;
          
            BasketItemAddedEvent basketItemAddedEvent = new BasketItemAddedEvent()
            {
                Category = (Category)Catagory0,
                Price = Price0.ToString(),
                Product = Product0.ToString()
            };
            await publishEndpoint.Publish(basketItemAddedEvent);

            return RedirectToAction(actionName:"Menu",controllerName:"Home");
        }





        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
