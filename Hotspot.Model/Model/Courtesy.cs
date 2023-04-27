using System;
using System.Collections.Generic;
using System.Text;

namespace Hotspot.Model.Model
{
    public class Courtesy : Password
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Description { get; set; }
    }
}
