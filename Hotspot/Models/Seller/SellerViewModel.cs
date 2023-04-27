using Hotspot.Models.Locale;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hotspot.Models.Seller
{
    public class SellerViewModel
    {
        public SellerViewModel()
        {

        }
        
        public SellerViewModel(string name, string surname, string email, string cpf, 
                                string rg, char sex, DateTime birthday, string street, 
                                string number, string neighborhood, 
                                string city, string state)
        {
            Name = name;
            Surname = surname;
            Email = email;
            Cpf = cpf;
            Rg = rg;
            Sex = sex;
            Birthday = birthday;
            Street = street;
            Number = number;
            Neighborhood = neighborhood;
            City = city;
            State = state;
        }

        //System Info
        public int Id { get; set; }

        //Personal Info
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Cpf { get; set; }
        public string Rg { get; set; }
        public char Sex { get; set; }
        public DateTime Birthday { get; set; }
        public string PhoneNumber1 { get; set; }
        public string PhoneNumber2 { get; set; }
        public bool HasPendency { get; set; }

        //Address Info
        public string Street { get; set; }
        public string Number { get; set; }
        public string Neighborhood { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int LocaleId { get; set; }

        //Batch Info
        public IEnumerable<SellerBatchViewModel> Batches { get; set; }
        public IEnumerable<SellerCatalogBatchViewModel> CatalogBatches { get; set; }
    }
}
