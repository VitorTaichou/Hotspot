using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hotspot.Models.Seller
{
    public class SellerListObject
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Cpf { get; set; }

        public string Locale { get; set; }
    }
}
