using Hotspot.Model;
using Hotspot.Model.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hotspot.Services
{
    public class LogService : ILog
    {
        private readonly HotspotContext _context;

        public LogService(HotspotContext context)
        {
            _context = context;
        }

        public async Task Add(Log newLog)
        {
            await _context.Log.AddAsync(newLog);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<Log> GetAll()
        {
            return _context.Log;
        }
    }
}
