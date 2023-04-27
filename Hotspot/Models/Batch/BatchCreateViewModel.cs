using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hotspot.Models.Batch
{
    public class BatchCreateViewModel
    {
        public BatchCreateViewModel() { }

        public BatchCreateViewModel(int sellerId)
        {
            SellerId = sellerId;
        }

        public string PaymentMethod { get; set; }
        public int SellerId { get; set; }
        public decimal Commission { get; set; }

        public int OneHourQty { get; set; }
        public int TwoHourQty { get; set; }
        public int ThreeHourQty { get; set; }
        public int SixHourQty { get; set; }

        public int CashFlowId { get; set; }
    }
}
