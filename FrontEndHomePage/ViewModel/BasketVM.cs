using Shared.Enums;

namespace FrontEndHomePage.ViewModel
{
    public class BasketVM
    {
        public int   Id { get; set; }

        public Category Category { get; set; }

        public string Price { get; set; }

        public string Product { get; set; }

        public string TotalPrice { get; set; }
    }
}
