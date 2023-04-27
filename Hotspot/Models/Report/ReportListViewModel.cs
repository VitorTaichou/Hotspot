using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hotspot.Models.Report
{
    public class ReportListViewModel
    {
        public IEnumerable<ReportObjectViewModel> DayPendencies { get; set; }
        public IEnumerable<ReportObjectViewModel> DayPaid { get; set; }
        public IEnumerable<ReportObjectViewModel> MonthPendencies { get; set; }
        public IEnumerable<ReportObjectViewModel> MonthPaid { get; set; }
        public IEnumerable<ReportObjectViewModel> LastMonthPendencies { get; set; }
        public IEnumerable<ReportObjectViewModel> LastMonthPaid { get; set; }
    }
}
