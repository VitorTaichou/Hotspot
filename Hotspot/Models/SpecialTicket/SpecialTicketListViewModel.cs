using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hotspot.Models.SpecialTicket
{
    public class SpecialTicketListViewModel
    {
        public int Id { get; set; }
        public int CashFlowId { get; set; }
        public IEnumerable<SpecialTicketViewModel> SpecialTickets { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
