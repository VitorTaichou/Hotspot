using System.Collections.Generic;

namespace Hotspot.Models.CatalogTicket
{
    public class CatalogTicketItemListViewModel
    {
        public List<CatalogTicketItemViewModel> ItemsList { get; set; }
        public int SellerId { get; set; }
        public string SellerName { get; set; }
        public string SellerCity { get; set; }
        public int Comission { get; set; }
        public string PaymentMethod { get; set; }
        public bool Payment { get; set; }
    }
}
