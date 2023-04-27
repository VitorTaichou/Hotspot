using Hotspot.Model.Model;
using System;
using System.Threading.Tasks;

namespace Hotspot.Model
{
    public interface ITicket
    {
        Task<Ticket> GetByPassword(string code);
        Task SetTime(string code, TimeSpan time);
        Task UpdateTimeByPassword(string code, int timeSeconds);
        Task SetTimeByPassword(string code, int timeSeconds);
    }
}
