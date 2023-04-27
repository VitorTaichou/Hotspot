using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Hotspot.Model.Model
{
    public class Log
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string User { get; set; }
        public DateTime Time { get; set; }
        public string Action { get; set; }
    }
}
