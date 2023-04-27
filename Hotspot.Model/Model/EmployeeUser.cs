using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Hotspot.Model.Model
{
    public class EmployeeUser : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Cpf { get; set; }
        public string Rg { get; set; }
        public char Sex { get; set; }
        public DateTime Birthday { get; set; }
        public bool Active { get; set; }

        public virtual Address Address { get; set; }
        public virtual IEnumerable<Phonenumber> Phonenumbers { get; set; }
        public virtual IEnumerable<Batch> SelledBatches { get; set; }
    }
}