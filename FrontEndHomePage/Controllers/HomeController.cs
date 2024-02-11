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
                    Product ="0"
                });
            }
        

            return View(basketVM);
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
