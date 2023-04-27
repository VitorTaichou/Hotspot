using System;

namespace Hotspot.Models.Seller
{
    public class SellerCatalogBatchViewModel
    {
        public int Id { get; set; }
        public string PaymentMethod { get; set; }
        public bool Payment { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalValue { get; set; }
        public decimal PaidValue { get; set; }
        public decimal Comission { get; set; }
    }
}
