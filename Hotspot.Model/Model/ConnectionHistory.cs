using System;
using System.Collections.Generic;
using System.Text;

namespace Hotspot.Model.Model
{
    public class ConnectionHistory
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public DateTime ConnectionTime { get; set; }
        public TimeSpan TimeLeft { get; set; }
    }
}
