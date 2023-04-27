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
    public class BatchService : IBatch
    {
        private readonly HotspotContext _context;
        private readonly IGenerator _generatorService;

        public BatchService(HotspotContext context, IGenerator generatorService)
        {
            _context = context;
            _generatorService = generatorService;
        }

        public async Task AddFlowToBatch(Flow flow, Batch batch)
        {
            batch.FlowList.Concat(new Flow[] { flow });
            await _context.SaveChangesAsync();
        }

        public async Task<bool> Create(Batch newBatch)
        {
            await _context.AddAsync(newBatch);
            await _context.SaveChangesAsync();

            //Generate Code Logic
            foreach (var ticket in newBatch.TicketList)
            {
                ticket.Code = _generatorService.GenerateEightDigit(ticket.Id);
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task Delete(int id)
        {
            var batchToRemove = await GetById(id);

            if(batchToRemove != null)
            {
                //Removing Flow and sale
                foreach(var flow in batchToRemove.FlowList)
                {
                    if (flow != null)
                    {
                        //Removing Flow
                        _context.Flow.Remove(flow);
                    }
                }

                //Removing Tickets
                foreach (var ticket in batchToRemove.TicketList)
                {
                    if (ticket != null)
                    {
                        _context.Ticket.Remove(ticket);
                    }
                }
                
                _context.Batch.Remove(batchToRemove);
                await _context.SaveChangesAsync();
            }
        }

        public IEnumerable<Batch> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Batch> GetAllFromSeller(int sellerId)
        {
            throw new NotImplementedException();
        }

        public async Task<Batch> GetById(int id)
        {
            return await _context.Batch.Where(b => b.Id == id)
                .Include(b => b.Seller).ThenInclude(s => s.Address).ThenInclude(a => a.Locale)
                .Include(b => b.TicketList)
                .Include(b => b.EmployeeUser)
                .Include(b => b.FlowList)
                .Include(b => b.CashFlow)
                .FirstAsync();
        }

        public async Task<Batch> GetBySale(int id)
        {
            //var sale = await _context.Sale.Where(s => s.Id == id).Include(s => s.Batch).FirstAsync();
            //return await this.GetById(sale.Batch.Id);

            throw new NotImplementedException();
        }

        public async Task UpdateTickets(Batch batch)
        {
            var b = _context.Batch.Where(b => b.Id == batch.Id).Include(b => b.TicketList).FirstOrDefault();

            foreach(var newTicket in batch.TicketList)
            {
                var ticket = b.TicketList.Where(t => t.Id == newTicket.Id).FirstOrDefault();
                ticket.Code = newTicket.Code;
            }

            await _context.SaveChangesAsync();
        }
    }
}
