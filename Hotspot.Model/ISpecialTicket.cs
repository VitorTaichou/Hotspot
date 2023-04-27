using Hotspot.Model.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hotspot.Model
{
    public interface ISpecialTicket
    {
        Task Add(SpecialTicket specialTicket);
        Task Delete(int id);

        Task SetFirstUse(int id);
        Task SetFirstUse(SpecialTicket specialTicket);
        Task<SpecialTicket> GetById(int id);
        Task<SpecialTicket> GetByPassword(string password);
        IEnumerable<SpecialTicket> GetBySellerId(int id);
    }
}
