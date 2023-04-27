using Hotspot.Models.CashFlow;
using System;
using System.ComponentModel.DataAnnotations;

namespace Hotspot.Models.Flow
{
    public class FlowViewModel
    {
        public string Id { get; set; }
        [DisplayFormat(DataFormatString = "{0:[C]}", ApplyFormatInEditMode = true)]
        public string Amount { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; }

        public CashFlowViewModel CashFlow { get; set; }
        public int CashFlowId { get; set; }
        public string EmployeeName { get; set; }
    }
}
