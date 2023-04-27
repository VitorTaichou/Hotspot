using Hotspot.Model;
using Hotspot.Model.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Hotspot.Services
{
    public class TicketService : ITicket
    {
        private readonly HotspotContext _context;

        public TicketService(HotspotContext context)
        {
            this._context = context;
        }
        
        public async Task<Ticket> GetByPassword(string code)
        {
            try
            {
                return await _context.Ticket.Where(t => t.Code == code)
                    .Include(t => t.Batch)
                    .ThenInclude(b => b.Seller).ThenInclude(s => s.Address).ThenInclude(a => a.Locale)
                    .FirstAsync();
            }
            catch(Exception e)
            {
                return null;
            }
        }

        public async Task SetTime(string code, TimeSpan time)
        {
            try
            {
                var ticket = await _context.Ticket.Where(t => t.Code == code).Include(t => t.Batch).FirstAsync();
                if (ticket != null)
                {
                    ticket.Time = time;
                    await _context.SaveChangesAsync();
                }
            }
            catch(Exception e)
            {

            }
        }

        public async Task SetTimeByPassword(string code, int timeSeconds)
        {
            var ticket = await GetByPassword(code);
            TimeSpan newTime = TimeSpan.FromSeconds(timeSeconds);
            ticket.Time = newTime;

            await _context.SaveChangesAsync();
        }

        public async Task UpdateTimeByPassword(string code, int timeSeconds)
        {
            var ticket = await GetByPassword(code);
            double secsLeft = ticket.Time.TotalSeconds;
            double secsToTake = (double) timeSeconds;
            secsLeft -= secsToTake;

            if(secsLeft < 0)
            {
                secsLeft = 0;
            }

            TimeSpan newTime = TimeSpan.FromSeconds(secsLeft);
            ticket.Time = newTime;

            await _context.SaveChangesAsync();
        }
    }
}
