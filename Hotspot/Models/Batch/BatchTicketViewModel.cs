using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hotspot.Models.Batch
{
    public class BatchTicketViewModel
    {
        public string Code { get; set; }
        public string Value { get; set; }
        public TimeSpan TimeLeft { get; set; }
        public DateTime DueDate { get; set; }
        public string SellerName { get; set; }
        public string SellerCity { get; set; }
    }
}
