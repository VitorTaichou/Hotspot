using Hotspot.Model.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hotspot.Model
{
    public interface ICatalogTicketItem
    {
        //CRUD
        public Task Create(CatalogTicketItem newItem);
        public IEnumerable<CatalogTicketItem> GetAll();
        public CatalogTicketItem Get(int id);
        public Task Update(CatalogTicketItem item);
        public void Remove(int id);
    }
}
