using Hotspot.Model;
using Hotspot.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hotspot.Services
{
    public class CatalogTicketItemService : ICatalogTicketItem
    {
        private readonly HotspotContext _context;

        public CatalogTicketItemService(HotspotContext context)
        {
            _context = context;
        }

        public async Task Create(CatalogTicketItem newItem)
        {
            await _context.CatalogTicketItem.AddAsync(newItem);
            await _context.SaveChangesAsync();
        }

        public CatalogTicketItem Get(int id)
        {
            try
            {
                return _context.CatalogTicketItem.Where(i => i.Id == id).FirstOrDefault();
            }
            catch(Exception e)
            {
                return null;
            }
        }

        public IEnumerable<CatalogTicketItem> GetAll()
        {
            return _context.CatalogTicketItem;
        }

        public void Remove(int id)
        {
            _context.Remove(this.Get(id));
            _context.SaveChanges();
        }

        public async Task Update(CatalogTicketItem item)
        {
            var old = this.Get(item.Id);

            old.Time = item.Time;
            old.Value = item.Value;
            old.Bandwidth = item.Bandwidth;
            old.Color = item.Color;
            old.Description = item.Description;
            old.ExpireDays = item.ExpireDays;
            old.Franchise = item.Franchise;


            await _context.SaveChangesAsync();
        }
    }
}
