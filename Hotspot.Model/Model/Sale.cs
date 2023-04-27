using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Hotspot.Model.Model
{
    public class Sale
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int QtyOneHour { get; set; }
        public int QtyTwoHour { get; set; }
        public int QtyThreeHour { get; set; }
        public int QtySixHour { get; set; }

        public decimal TotalValue { get; set; }
        public decimal PaidValue { get; set; }
        public bool Payment { get; set; } //Pago, Não Pago
        public string PaymentMethod { get; set; } //A Vista, A Prazo

        //public virtual Batch Batch { get; set; }
        public virtual EmployeeUser Employee { get; set; }
        public virtual Seller Seller { get; set; }
        public virtual Flow Flow { get; set; }
    }
}
