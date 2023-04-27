using System;
using System.Collections.Generic;

namespace Hotspot.Model.Model
{
    public class Seller
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Cpf { get; set; }
        public string Rg { get; set; }
        public string Email { get; set; }
        public char Sex { get; set; }
        public DateTime Birthday { get; set; }

        public virtual Address Address { get; set; }
        public virtual IEnumerable<Phonenumber> Phonenumbers { get; set; }
        public virtual IEnumerable<Batch> Batches { get; set; }
        public virtual IEnumerable<CatalogBatch> CatalogBatches { get; set; }
    }
}
