using System;
using System.Collections.Generic;

namespace Hotspot.Models.Flow
{
    public class FlowListViewModel
    {
        public IEnumerable<FlowViewModel> FlowViewModelList { get; set; }

        public decimal TotalAmount { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public decimal TotalOutflow { get; set; }
        public decimal TotalInflow { get; set; }
        public decimal CurrentAmount { get; set; }
    }
}
