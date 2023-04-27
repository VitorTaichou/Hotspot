using System.Collections.Generic;

namespace Hotspot.Models.Seller
{
    public class SellerListViewModel
    {
        public IEnumerable<SellerListObject> Sellers { get; set; }
        public string Search { get; set; }
        public int LocaleId { get; set; }
    }
}
