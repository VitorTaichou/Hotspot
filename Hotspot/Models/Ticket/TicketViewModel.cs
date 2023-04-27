using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hotspot.Models.Ticket
{
    public class TicketViewModel
    {
        public string Code { get; set; }
        public string SellerName { get; set; }
        public int BatchId { get; set; }
        public TimeSpan TimeLeft { get; set; }
        public string Franchise { get; set; }

        public IEnumerable<TicketConnectionHistoryViewModel> ConnectionHistory { get; set; }
        public IEnumerable<TicketLogoutHistoryViewModel> LogoutHistory { get; set; }
    }
}
