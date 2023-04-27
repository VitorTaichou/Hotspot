using Hotspot.Model;
using Hotspot.Model.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hotspot.Services
{
    public class HistoryService : IHistory
    {
        private readonly HotspotContext _context;

        public HistoryService(HotspotContext context)
        {
            _context = context;
        }

        public IEnumerable<ConnectionHistory> GetConnectionHistoryByTicket(string ticket)
        {
            return _context.ConnectionHistory.Where(c => c.Code == ticket);
        }

        public IEnumerable<LogoutHistory> GetLogoutHistoryByTicket(string ticket)
        {
            return _context.LogoutHistory.Where(l => l.Code == ticket);
        }

        public async Task RegisterConnection(ConnectionHistory connectionHistory)
        {
            await _context.ConnectionHistory.AddAsync(connectionHistory);
            await _context.SaveChangesAsync();
        }

        public async Task RegisterLogout(LogoutHistory logoutHistory)
        {
            await _context.LogoutHistory.AddAsync(logoutHistory);
            await _context.SaveChangesAsync();
        }
    }
}
