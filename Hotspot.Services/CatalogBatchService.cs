using Hotspot.Model;
using Hotspot.Model.Model;
using Hotspot.Tools.Code;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hotspot.Services
{
    public class CatalogBatchService : ICatalogBatch
    {
        private readonly HotspotContext _context;
        private readonly IGenerator _generatorService;

        public CatalogBatchService(HotspotContext context, IGenerator generatorService)
        {
            _context = context;
            _generatorService = generatorService;
        }

        public async Task AddFlowToBatch(Flow flow, CatalogBatch batch)
        {
            batch.FlowList.Concat(new Flow[] { flow });
            await _context.SaveChangesAsync();
        }

        public async Task<bool> Create(CatalogBatch newBatch)
        {
            await _context.CatalogBatch.AddAsync(newBatch);
            _context.SaveChanges();

            foreach(var ticket in newBatch.TicketList)
            {
                ticket.Code = _generatorService.GenerateNineDigit(ticket.Id);
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task Delete(int id)
        {
            var cBatch = await this.GetById(id);

            try
            {
                foreach(var flow in cBatch.CashFlow.Flows.Where(f => f.CatalogBatch.Id == cBatch.Id))
                {
                    _context.Remove(flow);
                }
            }
            catch (Exception e) { }

            _context.Remove(cBatch);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<CatalogBatch> GetAllFromSeller(int sellerId)
        {
            return _context.CatalogBatch.Where(c => c.Seller.Id == sellerId)
                .Include(c => c.Seller)
                .Include(c => c.CashFlow).ThenInclude(c => c.Flows)
                .Include(c => c.EmployeeUser)
                .Include(c => c.TicketList)
                .Include(c => c.FlowList);
        }

        public Task<CatalogBatch> GetById(int id)
        {
            try
            {
                return _context.CatalogBatch.Where(c => c.Id == id)
                    .Include(c => c.Seller).ThenInclude(s => s.Address).ThenInclude(a => a.Locale)
                    .Include(c => c.CashFlow)
                    .Include(c => c.EmployeeUser)
                    .Include(c => c.TicketList)
                    .Include(c => c.FlowList).FirstOrDefaultAsync();
            }
            catch(Exception e)
            {
                return null;
            }
        }

        public Task UpdateTickets(CatalogBatch batch)
        {
            throw new NotImplementedException();
        }
    }
}
