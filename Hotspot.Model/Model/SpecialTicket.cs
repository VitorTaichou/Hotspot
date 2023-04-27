using System;
using System.Collections.Generic;
using System.Text;

namespace Hotspot.Model.Model
{
    public class SpecialTicket : Password
    {
        public DateTime DueDate { get; set; }
        public bool FirstUse { get; set; }

        public virtual Seller Seller { get; set; }
        public virtual Flow Flow { get; set; }
    }
}
