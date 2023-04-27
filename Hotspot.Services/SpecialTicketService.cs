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
    public class SpecialTicketService : ISpecialTicket
    {
        private readonly HotspotContext _context;
        private readonly IGenerator _generatorService;

        public SpecialTicketService(HotspotContext context, IGenerator generatorService)
        {
            _context = context;
            _generatorService = generatorService;
        }

        public async Task Add(SpecialTicket specialTicket)
        {
            _context.SpecialTicket.Add(specialTicket);
            await _context.SaveChangesAsync();

            //Generate Code
            specialTicket.Code = _generatorService.GenerateSevenDigit(specialTicket.Id);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            _context.SpecialTicket.Remove(await GetById(id));
            await _context.SaveChangesAsync();
        }

        public async Task<SpecialTicket> GetById(int id)
        {
            return await _context.SpecialTicket
                .Where(s => s.Id == id)
                .Include(s => s.Seller).ThenInclude(s => s.Address).ThenInclude(a => a.Locale)
                .Include(s => s.Flow)
                .FirstAsync();
        }

        public async Task<SpecialTicket> GetByPassword(string password)
        {
            try
            {
                return await _context.SpecialTicket.Where(s => s.Code == password).FirstOrDefaultAsync();
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<SpecialTicket> GetBySellerId(int id)
        {
            return _context.SpecialTicket
                .Where(s => s.Seller.Id == id)
                .Include(s => s.Seller).ThenInclude(s => s.Address).ThenInclude(a => a.Locale)
                .Include(s => s.Flow);
        }

        public async Task SetFirstUse(int id)
        {
            var t = await GetById(id);
            t.FirstUse = true;
            await _context.SaveChangesAsync();
        }

        public async Task SetFirstUse(SpecialTicket specialTicket)
        {
            specialTicket.FirstUse = true;
            specialTicket.DueDate = DateTime.Now.AddDays(7);
            await _context.SaveChangesAsync();
        }
    }
}
