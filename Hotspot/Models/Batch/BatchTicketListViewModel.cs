using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hotspot.Models.Batch
{
    public class BatchTicketListViewModel
    {
        public IEnumerable<BatchTicketViewModel> TicketList { get; set; }
    }
}
