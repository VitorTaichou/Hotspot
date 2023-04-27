using Hotspot.Model.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hotspot.Model
{
    public interface ISeller
    {
        Task<Seller> GetById(int id);
        IEnumerable<Seller> GetAll();
        IEnumerable<Seller> Search(string search);

        Task Create(Seller seller);
        Task Edit(Seller seller);
        Task Delete(int id);
    }
}
