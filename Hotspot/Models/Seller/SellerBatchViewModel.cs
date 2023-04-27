using System;

namespace Hotspot.Models.Seller
{
    public class SellerBatchViewModel
    {
        public SellerBatchViewModel() { }

        public SellerBatchViewModel(int id, int oneHourQty, int twoHourQty, int threeHourQty, int sixHourQty, string paymentMethod)
        {
            Id = id;
            OneHourQty = oneHourQty;
            TwoHourQty = twoHourQty;
            ThreeHourQty = threeHourQty;
            SixHourQty = sixHourQty;
            PaymentMethod = paymentMethod;
        }

        public int Id { get; set; }
        public string PaymentMethod { get; set; }
        public bool Payment { get; set; }
        public int OneHourQty { get; set; }
        public int TwoHourQty { get; set; }
        public int ThreeHourQty { get; set; }
        public int SixHourQty { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalValue { get; set; }
        public decimal PaidValue { get; set; }
        public decimal Comission { get; set; }
    }
}