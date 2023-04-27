using System;
using System.Collections.Generic;

namespace Hotspot.Model.Model
{
    public class CatalogBatch
    {
        public int Id { get; set; }
        public string PaymentMethod { get; set; }
        public decimal Commission { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalValue { get; set; }
        public bool Payment { get; set; }

        public virtual Seller Seller { get; set; }
        public virtual CashFlow CashFlow { get; set; }
        public virtual EmployeeUser EmployeeUser { get; set; }
        public virtual IEnumerable<CatalogTicket> TicketList { get; set; }
        public virtual IEnumerable<Flow> FlowList { get; set; }
    }
}
