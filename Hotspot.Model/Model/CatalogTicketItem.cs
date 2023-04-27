using System;

namespace Hotspot.Model.Model
{
    public class CatalogTicketItem
    {
        public int Id { get; set; }
        public decimal Value { get; set; }
        public long Time { get; set; }
        public string Bandwidth { get; set; }
        public long Franchise { get; set; }
        public string Color { get; set; }
        public string Description { get; set; }
        public int ExpireDays { get; set; }
    }
}
