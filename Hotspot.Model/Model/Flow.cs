using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotspot.Model.Model
{
    public class Flow
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public virtual Batch Batch { get; set; }
        public virtual CatalogBatch CatalogBatch { get; set; }
        public virtual CashFlow CashFlow { get; set; }
        public virtual EmployeeUser EmployeeUser { get; set; }
    }
}
