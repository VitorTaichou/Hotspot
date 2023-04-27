using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hotspot.Models.Ticket
{
    public class TicketLogoutHistoryViewModel
    {
        public DateTime LogoutTime { get; set; }
        public TimeSpan TimeUsed { get; set; }
    }
}
