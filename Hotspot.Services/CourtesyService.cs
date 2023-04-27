using Hotspot.Model;
using Hotspot.Model.Model;
using Hotspot.Tools.Code;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotspot.Services
{
    public class CourtesyService : ICourtesy
    {
        private readonly HotspotContext _context;
        private readonly IGenerator _generatorService;

        public CourtesyService(HotspotContext context, IGenerator generatorService)
        {
            _context = context;
            _generatorService = generatorService;
        }

        public async Task Add(Courtesy courtesy)
        {
            await _context.AddAsync(courtesy);
            await _context.SaveChangesAsync();

            //GenerateCode
            courtesy.Code = _generatorService.GenerateCourtesy(courtesy.Id);
            await _context.SaveChangesAsync();
        }

        public async Task<Courtesy> GetById(int id)
        {
            try
            {
                var courtesy = await _context.Coutesy.Where(c => c.Id == id).FirstAsync();
                return courtesy;
            }
            catch(Exception e)
            {
                return null;
            }
        }

        public async Task Delete(int id)
        {
            var courtesyToDelete = await GetById(id);

            if(courtesyToDelete != null)
            {
                _context.Remove(courtesyToDelete);
                await _context.SaveChangesAsync();
            }
        }

        public IEnumerable<Courtesy> GetAll()
        {
            return _context.Coutesy;
        }

        public IEnumerable<Courtesy> Search(string search)
        {
            var list = _context.Coutesy.Where(c => c.Name.Contains(search) || c.Surname.Contains(search) || c.Description.Contains(search));
            return list;
        }

        public async Task Edit(Courtesy courtesy)
        {
            if(courtesy != null)
            {
                var c = await _context.Coutesy.Where(c => c.Id == courtesy.Id).FirstOrDefaultAsync();
                c.Description = courtesy.Description;
                c.Name = courtesy.Name;
                c.Surname = courtesy.Surname;

                //Save
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Courtesy> GetByPassword(string password)
        {
            var ticket = await _context.Coutesy.Where(c => c.Code == password).FirstOrDefaultAsync();
            return ticket;
        }
    }
}
