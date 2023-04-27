using Hotspot.Model.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hotspot.Model
{
    public interface IBatch
    {
        Task<Batch> GetById(int id);
        IEnumerable<Batch> GetAllFromSeller(int sellerId);
        IEnumerable<Batch> GetAll();

        Task<Batch> GetBySale(int id);

        Task<bool> Create(Batch newBatch);
        Task Delete(int id);
        Task UpdateTickets(Batch batch);

        Task AddFlowToBatch(Flow flow, Batch batch);
    }
}
