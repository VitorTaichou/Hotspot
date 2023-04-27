using System;
using System.Collections.Generic;
using System.Text;

namespace Hotspot.Model.Model
{
    public class LogoutHistory
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public DateTime LogoutTime { get; set; }
        public TimeSpan TimeUsed { get; set; }
    }
}
