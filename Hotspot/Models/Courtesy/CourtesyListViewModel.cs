using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hotspot.Models.Courtesy
{
    public class CourtesyListViewModel
    {
        public IEnumerable<CourtesyViewModel> Courtesies { get; set; }
        public string Search { get; set; }
    }
}
