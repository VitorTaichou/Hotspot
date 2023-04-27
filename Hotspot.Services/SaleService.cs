using Hotspot.Model;
using Hotspot.Model.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hotspot.Services
{
    public class SaleService : ISale
    {
        private readonly HotspotContext _context;

        public SaleService(HotspotContext context)
        {
            _context = context;
        }

        public async Task Add(Sale sale)
        {
            await _context.AddAsync(sale);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var sale = _context.Sale.Where(s => s.Id == id).FirstOrDefault();
            _context.Remove(sale);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteByBatch(Batch batch)
        {
            //var sale = _context.Sale.Where(s => s.Batch.Id == batch.Id).Include(s => s.Batch);
            /*
            var batchFind = await _context.Batch.Where(b => b.Id == batch.Id).Include(b => b.Sale).FirstAsync();
            _context.Remove(batchFind.Sale);
            await _context.SaveChangesAsync();
             */

            throw new NotImplementedException();
        }

        public IEnumerable<Sale> GetAllByDay(DateTime date)
        {
            var sales = _context.Sale
                .Where(s => s.Date.Year == date.Year)
                .Where(s => s.Date.Month == date.Month)
                .Where(s => s.Date.Day == date.Day)
                .Include(s => s.Seller).ThenInclude(s => s.Address).ThenInclude(a => a.Locale);

            return sales;
        }

        public IEnumerable<Sale> GetAllByEmployee(string employeeId)
        {
            return _context.Sale
                .Where(s => s.Employee.Id == employeeId)
                .Include(s => s.Employee)
                .Include(s => s.Seller).ThenInclude(s => s.Address).ThenInclude(a => a.Locale);
        }

        public IEnumerable<Sale> GetAllByMonth(int month, int year)
        {
            var sales = _context.Sale
                .Where(s => s.Date.Month == month)
                .Where(s => s.Date.Year == year)
                .Include(s => s.Seller).ThenInclude(s => s.Address).ThenInclude(a => a.Locale);

            return sales;
        }

        public IEnumerable<Sale> GetAllBySeller(int sellerId)
        {
            return _context.Sale.Where(s => s.Seller.Id == sellerId)
                .Include(s => s.Seller).ThenInclude(s => s.Address).ThenInclude(a => a.Locale)
                .Include(s => s.Employee);
        }

        public async Task<IEnumerable<Sale>> GetByBatchId(int id)
        {
            var batch = await _context.Batch.Where(b => b.Id == id)
                .Include(b => b.FlowList)
                .FirstAsync();

            throw new NotImplementedException();
        }

        public async Task<Sale> GetById(int id)
        {
            return await _context.Sale.Where(s => s.Id == id)
                .Include(s => s.Employee)
                .Include(s => s.Flow)
                .Include(s => s.Seller).ThenInclude(s => s.Address).ThenInclude(a => a.Locale)
                .FirstOrDefaultAsync();
        }

        public async Task InformPayment(int id, decimal value)
        {
            /*
            //Get Sale
            var sale = await this.GetById(id);

            //Add Paid value to the Sale
            if(sale.PaidValue + value >= sale.TotalValue)
            {
                sale.PaidValue = sale.TotalValue;
            }
            else
            {
                sale.PaidValue += value;
            }

            //Verify if the total paid is equal to the total value, if yes, then set it to paid
            if(sale.PaidValue >= sale.TotalValue)
            {
                sale.Payment = true;
            }

            //Register FLow
            var batch = await _context.Batch.Where(b => b.Sale.Id == id).Include(b => b.CashFlow).FirstAsync();
            _context.Flow.Add(new Flow()
            {
                Amount = value,
                Date = DateTime.Now,
                Description = "Pagamento do Lote " + batch.Id,
                CashFlow = batch.CashFlow
            });

            await _context.SaveChangesAsync();
            */

            throw new NotImplementedException();
        }

        public async Task InformPaymentByBatchId(int id, decimal value)
        {
            /*
            var sale = await this.GetByBatchId(id);
            sale.Concat(new Sale[] { new Sale()
            {
                Batch = await _context.Batch.Where(b => b.Id == id).FirstAsync(),
                Date = DateTime.Now,
                
                
            }});
            sale.Payment = true;
            await _context.SaveChangesAsync();
            */

            throw new NotImplementedException();
        }

        Task<Sale> ISale.GetByBatchId(int id)
        {
            throw new NotImplementedException();
        }
    }
}
