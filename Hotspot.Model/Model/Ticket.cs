using System;
using System.Collections.Generic;
using System.Text;

namespace Hotspot.Model.Model
{
    public class Ticket : Password
    {
        public Ticket()
        {
        }

        public Ticket(double value, TimeSpan time, Batch batch)
        {
            Value = value;
            Time = time;
            Batch = batch;
        }

        public double Value { get; set; }
        public TimeSpan Time { get; set; }
        public string Bandwidth { get; set; }
        public long Franchise { get; set; }

        public virtual Batch Batch { get; set; }
    }
}
