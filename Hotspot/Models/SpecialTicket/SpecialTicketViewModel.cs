using System;

namespace Hotspot.Models.SpecialTicket
{
    public class SpecialTicketViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int SellerId { get; set; }
        public DateTime DueDate { get; set; }
        public bool FirstUse { get; set; }
        public string SellerName { get; set; }
        public string SellerCity { get; set; }
    }
}
