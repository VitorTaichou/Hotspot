using System;

namespace Hotspot.Model.Model
{
    public class CatalogTicket : Password
    {
        public decimal Value { get; set; }
        public long Time { get; set; }
        public string Bandwidth { get; set; }
        public long Franchise { get; set; }
        public int ExpirationDays { get; set; }
        public string Color { get; set; }
        public DateTime DueDate { get; set; }
        public bool FirstUse { get; set; }
        public string ProfileName { get; set; }

        public virtual CatalogBatch Batch { get; set; }
    }
}
