using System;
using System.Collections.Generic;
using System.Text;

namespace Hotspot.Model.Model
{
    public class CashFlow
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual Locale Locale { get; set; }
        public virtual IEnumerable<Flow> Flows { get; set; }
    }
}
