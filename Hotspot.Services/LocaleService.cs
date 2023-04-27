using Hotspot.Model;
using Hotspot.Model.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hotspot.Services
{
    public class LocaleService : ILocale
    {
        private readonly HotspotContext _context;

        public LocaleService(HotspotContext context)
        {
            _context = context;
        }

        public async Task Add(Locale locale)
        {
            _context.Locale.Add(locale);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<Locale> GetAll()
        {
            return _context.Locale;
        }

        public async Task<Locale> GetById(int id)
        {
            return await _context.Locale.Where(l => l.Id == id).FirstAsync();
        }

        public async Task<Locale> GetByCity(string city)
        {
            return await _context.Locale.Where(l => l.City.Equals(city)).FirstAsync();
        }

        public async Task Remove(int id)
        {
            var locale = await this.GetById(id);
            _context.Locale.Remove(locale);
            await _context.SaveChangesAsync();
        }

        public async Task Remove(Locale locale)
        {
            _context.Locale.Remove(locale);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Locale locale)
        {
            var l = await this.GetById(locale.Id);

            l.City = locale.City;
            l.State = locale.State;
            l.Type = locale.Type;

            await _context.SaveChangesAsync();
        }
    }
}
