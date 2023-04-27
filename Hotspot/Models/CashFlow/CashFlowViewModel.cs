using Hotspot.Models.Locale;
using Hotspot.Models.Flow;
using System.Collections.Generic;
using System;

namespace Hotspot.Models.CashFlow
{
    public class CashFlowViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int LocaleId { get; set; }
        public LocaleViewModel LocaleViewModel { get; set; }
        public IEnumerable<FlowViewModel> FlowViewModelList { get; set; }

        public decimal TotalAmount { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public decimal TotalOutflow { get; set; }
        public decimal TotalInflow { get; set; }
        public decimal CurrentAmount { get; set; }
    }
}
