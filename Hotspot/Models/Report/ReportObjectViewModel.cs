using System;

namespace Hotspot.Models.Report
{
    public class ReportObjectViewModel
    {
        public string SellerFullName { get; set; }
        public string SellerCity { get; set; }
        public int BatchId { get; set; }
        public int SaleId { get; set; }
        public DateTime SaleDate { get; set; }
        public string PaymentMethod { get; set; }
        public double TotalPayment { get; set; }
    }
}
