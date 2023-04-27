using System;

namespace Hotspot.Models.CatalogTicket
{
    public class CatalogTicketItemViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public TimeSpan Time { get; set; }
        public int TimeDays { get; set; }
        public string Bandwidth { get; set; }
        public long Franchise { get; set; }
        public int ExpireDays { get; set; }
        public string Color { get; set; }
        public string Value { get; set; }
        public int Quantity { get; set; }
    }
}
