using Hotspot.Model.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hotspot.Model
{
    public interface ICourtesy
    {
        Task Add(Courtesy courtesy);
        Task Delete(int id);
        Task<Courtesy> GetById(int id);
        Task Edit(Courtesy courtesy);

        IEnumerable<Courtesy> GetAll();
        IEnumerable<Courtesy> Search(string search);
        Task<Courtesy> GetByPassword(string password);
    }
}
