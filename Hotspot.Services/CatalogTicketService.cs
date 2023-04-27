using Hotspot.Model;
using Hotspot.Model.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Hotspot.Services
{
    public class CatalogTicketService : ICatalogTicket
    {
        private readonly HotspotContext _context;

        public CatalogTicketService(HotspotContext context)
        {
            _context = context;
        }

        public CatalogTicket GetById(int id)
        {
            return _context.CatalogTicket.Where(t => t.Id == id).FirstOrDefault();
        }

        public CatalogTicket GetByPassword(string password)
        {
            return _context.CatalogTicket.Where(t => t.Code == password)
                .Include(t => t.Batch).ThenInclude(b => b.Seller)
                .FirstOrDefault();
        }

        public async Task<bool> UpdateTicketData(CatalogTicket ticket)
        {
            try
            {
                var t = this.GetById(ticket.Id);

                t.Time = ticket.Time;
                t.Franchise = ticket.Franchise;
            }
            catch (Exception e) { }

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task SetFirstUse(string password)
        {
            var t = this.GetByPassword(password);
            t.FirstUse = true;
            await _context.SaveChangesAsync();
        }

        public async Task SetFirstUse(int id)
        {
            var t = GetById(id);
            t.FirstUse = true;
            await _context.SaveChangesAsync();
        }

        public async Task SetFirstUse(CatalogTicket catalogTicket)
        {
            catalogTicket.FirstUse = true;
            catalogTicket.DueDate = DateTime.Now.AddDays(catalogTicket.ExpirationDays);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTicketData(string password, int time, string franchise)
        {
            var ticket = this.GetByPassword(password);

            //Time
            long secsLeft = ticket.Time;
            long secsToTake = (long) time;
            secsLeft -= secsToTake;

            if (secsLeft < 0)
            {
                secsLeft = 0;
            }

            ticket.Time = secsLeft;

            //Franchise
            long bytesLeft = ticket.Franchise;
            long bytesUsed = long.Parse(franchise);
            bytesLeft -= bytesUsed;

            if (bytesLeft < 0)
            {
                bytesLeft = 0;
            }
            ticket.Franchise = bytesLeft;

            await _context.SaveChangesAsync();
        }

        public async Task SetTicketData(string password, int time, string franchise)
        {
            var ticket = this.GetByPassword(password);

            //Time
            ticket.Time = time;

            //Franchise
            long bytesLeft = ticket.Franchise;
            long bytesUsed = long.Parse(franchise);
            bytesLeft -= bytesUsed;

            if (bytesLeft < 0)
            {
                bytesLeft = 0;
            }
            ticket.Franchise = bytesLeft;

            await _context.SaveChangesAsync();
        }
    }
}