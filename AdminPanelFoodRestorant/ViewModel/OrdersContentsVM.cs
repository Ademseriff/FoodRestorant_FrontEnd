using Shared.Enums;

namespace AdminPanelFoodRestorant.ViewModel
{
    public class OrdersContentsVM
    {
        public string TotalPrice { get; set; }

        public Category Category { get; set; }

        public string Product { get; set; }

        public string Email { get; set; }

        public int OrderId { get; set; }

        public string Adress { get; set; }

        public string PhoneNumber { get; set; }
    }
}
