using Hotspot.Model;
using Hotspot.Model.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hotspot.Services
{
    public class CashFlowService : ICashFlow
    {
        private readonly HotspotContext _context;

        public CashFlowService(HotspotContext context)
        {
            _context = context;
        }

        public async Task Add(CashFlow cashFlow)
        {
            _context.CashFlow.Add(cashFlow);
            await _context.SaveChangesAsync();
        }

        public async Task AddFlow(CashFlow cashFlow, Flow flow)
        {
            flow.CashFlow = cashFlow;
            _context.Flow.Add(flow);
            await _context.SaveChangesAsync();

            //Check Batch
            if (flow.Batch != null)
            {
                decimal total = flow.Batch.TotalValue;
                decimal paid = 0;

                foreach (var f in flow.Batch.FlowList)
                {
                    paid += f.Amount;
                }

                if (total <= paid)
                {
                    flow.Batch.Payment = true;
                    await _context.SaveChangesAsync();
                }
            }
            else if(flow.CatalogBatch != null)
            {
                decimal total = flow.CatalogBatch.TotalValue;
                decimal paid = 0;

                foreach (var f in flow.CatalogBatch.FlowList)
                {
                    paid += f.Amount;
                }

                if (total <= paid)
                {
                    flow.CatalogBatch.Payment = true;
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task AddFlow(int cashFlowId, Flow flow)
        {
            var cashFlow = await this.GetById(cashFlowId);
            flow.CashFlow = cashFlow;
            _context.Flow.Add(flow);

            await _context.SaveChangesAsync();

            //Check Batch
            if (flow.Batch != null)
            {
                decimal total = flow.Batch.TotalValue;
                decimal paid = 0;

                foreach (var f in flow.Batch.FlowList)
                {
                    paid += f.Amount;
                }

                if (total <= paid)
                {
                    flow.Batch.Payment = true;
                    await _context.SaveChangesAsync();
                }
            }
            else if (flow.CatalogBatch != null)
            {
                decimal total = flow.CatalogBatch.TotalValue;
                decimal paid = 0;

                foreach (var f in flow.CatalogBatch.FlowList)
                {
                    paid += f.Amount;
                }

                if (total <= paid)
                {
                    flow.CatalogBatch.Payment = true;
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task Delete(int id)
        {
            var cashFlow = await this.GetById(id);
            if(cashFlow.Flows == null || cashFlow.Flows.Count() == 0)
            {
                _context.CashFlow.Remove(cashFlow);
                await _context.SaveChangesAsync();
            }
        }

        public async Task Delete(CashFlow cashFlow)
        {
            if (cashFlow.Flows == null || cashFlow.Flows.Count() == 0)
            {
                _context.CashFlow.Remove(cashFlow);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteFlow(Flow flow)
        {
            _context.Flow.Remove(flow);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFlow(string id)
        {
            var flow = await this.GetFlowById(id);
            _context.Flow.Remove(flow);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<CashFlow> GetAll()
        {
            return _context.CashFlow.Include(c => c.Flows).Include(c => c.Locale);
        }

        public IEnumerable<Flow> GetAllFlow()
        {
            return _context.Flow
                .Include(f => f.CashFlow)
                .Include(f => f.EmployeeUser)
                .Include(f => f.Batch)
                .Include(f => f.CatalogBatch);
        }

        public async Task<CashFlow> GetById(int id)
        {
            return await _context.CashFlow.Where(c => c.Id == id)
                .Include(c => c.Flows).ThenInclude(f => f.EmployeeUser)
                .Include(c => c.Flows).ThenInclude(f => f.Batch)
                .Include(c => c.Flows).ThenInclude(f => f.CatalogBatch)
                .Include(c => c.Locale)
                .FirstAsync();
        }

        public IEnumerable<Flow> GetFlowByCashFlow(CashFlow cashFlow)
        {
            return _context.Flow.Where(f => f.CashFlow.Id == cashFlow.Id).Include(f => f.CashFlow)
                .Include(f => f.CashFlow).Include(f => f.EmployeeUser);
        }

        public IEnumerable<Flow> GetFlowByCashFlow(int cashFlowId)
        {
            return _context.Flow.Where(f => f.CashFlow.Id == cashFlowId)
                .Include(f => f.CashFlow).Include(f => f.EmployeeUser);
        }

        public IEnumerable<Flow> GetFlowByDate(int cashFlowId, DateTime startDate, DateTime endDate)
        {
            return _context.Flow.Where(f => f.Date >= startDate).Where(f => f.Date <= endDate).Where(f => f.CashFlow.Id == cashFlowId)
                .Include(f => f.CashFlow).Include(f => f.EmployeeUser);
        }

        public async Task<Flow> GetFlowById(string id)
        {
            return await _context.Flow.Where(f => f.Id.Equals(id))
                .Include(f => f.CashFlow).Include(f => f.EmployeeUser).FirstAsync();
        }
    }
}
