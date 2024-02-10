using FrontEndHomePage.Models;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Shared.Enums;
using Shared.Events;
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

        public IActionResult Sepet(int id)
        {

            return View();
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
