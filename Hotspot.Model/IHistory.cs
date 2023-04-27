using Hotspot.Model.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hotspot.Model
{
    public interface IHistory
    {
        Task RegisterConnection(ConnectionHistory connectionHistory);
        Task RegisterLogout(LogoutHistory logoutHistory);

        IEnumerable<ConnectionHistory> GetConnectionHistoryByTicket(string ticket);
        IEnumerable<LogoutHistory> GetLogoutHistoryByTicket(string ticket);
    }
}
