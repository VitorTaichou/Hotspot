using Hotspot.Model;
using Hotspot.Model.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hotspot.Services
{
    public class SellerService : ISeller
    {
        private readonly HotspotContext _context;
        
        public SellerService(HotspotContext context)
        {
            this._context = context;
        }

        public async Task Create(Seller seller)
        {
            await this._context.AddAsync(seller);
            await this._context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            //Seller to Remove
            var sellerToRemove = await GetById(id);

            //Removing Child Phones
            if(sellerToRemove.Phonenumbers != null)
            {
                var PhoneList = sellerToRemove.Phonenumbers;
                foreach (var phone in PhoneList)
                {
                    if (phone != null)
                    {
                        _context.Phonenumber.Remove(phone);
                    }
                }
            }

            //Removing Child Address
            if(sellerToRemove.Address != null)
            {
                var address = sellerToRemove.Address;
                if (address != null)
                {
                    _context.Address.Remove(address);
                }
            }

            if (sellerToRemove.Batches != null)
            {
                var batches = sellerToRemove.Batches;
                foreach(var batch in batches)
                {
                    if(batch != null)
                    {
                        foreach(var ticket in batch.TicketList)
                        {
                            if (ticket != null)
                            {
                                _context.Ticket.Remove(ticket);
                            }
                        }

                        foreach(var flow in batch.FlowList)
                        {
                            _context.Flow.Remove(flow);
                        }

                        _context.Batch.Remove(batch);
                    }
                }
            }

            //Removing Seller
            _context.Seller.Remove(_context.Seller.Where(s => s.Id == id).FirstOrDefault());
            await this._context.SaveChangesAsync();
        }

        public async Task Edit(Seller seller)
        {
            if(seller != null)
            {
                var s = await _context.Seller.Where(s => s.Id == seller.Id)
                    .Include(s => s.Phonenumbers)
                    .Include(s => s.Address).ThenInclude(a => a.Locale)
                    .FirstOrDefaultAsync();

                //Change current Info
                s.Name = seller.Name;
                s.Surname = seller.Surname;
                s.Birthday = seller.Birthday;
                s.Cpf = seller.Cpf;
                s.Rg = seller.Rg;
                s.Sex = seller.Sex;
                s.Email = seller.Email;

                //Change current Address
                s.Address.Street = seller.Address.Street;
                s.Address.Number = seller.Address.Number;
                s.Address.Neighborhood = seller.Address.Neighborhood;
                s.Address.Locale.City = seller.Address.Locale.City;
                s.Address.Locale.State = seller.Address.Locale.State;

                //Remove Phonenumbers and set New ones
                var phoneList = s.Phonenumbers;
                if(phoneList != null)
                {
                    foreach (var p in phoneList)
                    {
                        _context.Phonenumber.Remove(p);
                    }
                }

                s.Phonenumbers = seller.Phonenumbers;

                //Save
                await _context.SaveChangesAsync();
            }
        }

        public IEnumerable<Seller> GetAll()
        {
            return _context.Seller
                .Include(s => s.Batches)
                .Include(s => s.Address).ThenInclude(a => a.Locale);
        }

        public async Task<Seller> GetById(int id)
        {
            var seller = await _context.Seller.Where(s => s.Id == id)
                .Include(s => s.Address).ThenInclude(a => a.Locale)
                .Include(s => s.Phonenumbers)
                .Include(s => s.Batches).ThenInclude(b => b.TicketList)
                .Include(s => s.Batches).ThenInclude(b => b.FlowList)
                .Include(s => s.CatalogBatches).ThenInclude(b => b.TicketList)
                .Include(s => s.CatalogBatches).ThenInclude(b => b.FlowList)
                .FirstOrDefaultAsync();
            return seller;
        }

        public IEnumerable<Seller> Search(string search)
        {
            if(search != null)
            {
                if (!search.Equals(""))
                {
                    var list = _context.Seller
                        .Where(s => s.Name.Contains(search) || s.Surname.Contains(search) || s.Cpf.Contains(search))
                        .Include(s => s.Address).ThenInclude(a => a.Locale);
                    return list;
                }
                else
                {
                    var list = this.GetAll();
                    return list;
                }
            }
            else
            {
                var list = this.GetAll();
                return list;
            }
        }
    }
}
