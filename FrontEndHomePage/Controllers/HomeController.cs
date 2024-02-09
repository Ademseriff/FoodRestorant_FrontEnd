using FrontEndHomePage.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FrontEndHomePage.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
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

        public IActionResult SepetProcess(int id)
        {
            int Catagory,Price;
            Catagory = id % 10;
            Price = id / 10;

            return RedirectToAction(actionName:"Menu",controllerName:"Home");
        }





        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
