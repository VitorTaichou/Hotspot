using Hotspot.Model.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hotspot.Model
{
    public interface ICatalogBatch
    {
        Task<CatalogBatch> GetById(int id);
        IEnumerable<CatalogBatch> GetAllFromSeller(int sellerId);

        Task<bool> Create(CatalogBatch newBatch);
        Task Delete(int id);
        Task UpdateTickets(CatalogBatch batch);

        Task AddFlowToBatch(Flow flow, CatalogBatch batch);
    }
}
