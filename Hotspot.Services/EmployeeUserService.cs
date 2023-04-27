using Hotspot.Model;
using Hotspot.Model.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Hotspot.Services
{
    public class EmployeeUserService : IEmployeeUser
    {
        private readonly HotspotContext _context;

        public EmployeeUserService(HotspotContext context)
        {
            _context = context;
        }

        public async Task<EmployeeUser> GetById(string id)
        {
            return await _context.EmployeeUser.Where(e => e.Id == id).FirstOrDefaultAsync();
        }
    }
}
