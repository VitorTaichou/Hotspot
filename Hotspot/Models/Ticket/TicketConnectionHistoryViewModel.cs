using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hotspot.Models.Ticket
{
    public class TicketConnectionHistoryViewModel
    {
        public DateTime ConnectionTime { get; set; }
        public TimeSpan TimeLeft { get; set; }
    }
}
