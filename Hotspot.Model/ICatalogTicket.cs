using Hotspot.Model.Model;
using System.Threading.Tasks;

namespace Hotspot.Model
{
    public interface ICatalogTicket
    {
        public CatalogTicket GetById(int id);
        public CatalogTicket GetByPassword(string password);
        public Task UpdateTicketData(string password, int time, string franchise);
        public Task SetTicketData(string password, int time, string franchise);
        public Task SetFirstUse(int id);
        public Task SetFirstUse(string password);
        public Task SetFirstUse(CatalogTicket ticket);
    }
}
