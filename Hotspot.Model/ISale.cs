using Hotspot.Model.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hotspot.Model
{
    public interface ISale
    {
        Task Add(Sale sale);
        Task Delete(int id);
        Task DeleteByBatch(Batch batch);

        Task InformPayment(int id, decimal value);
        Task InformPaymentByBatchId(int id, decimal value);

        Task<Sale> GetById(int id);
        IEnumerable<Sale> GetAllBySeller(int sellerId);
        IEnumerable<Sale> GetAllByEmployee(string employeeId);
        IEnumerable<Sale> GetAllByDay(DateTime date);
        IEnumerable<Sale> GetAllByMonth(int month, int year);
        Task<Sale> GetByBatchId(int id);
    }
}
