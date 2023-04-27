using Hotspot.Model.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hotspot.Model
{
    public interface ILog
    {
        IEnumerable<Log> GetAll();
        Task Add(Log newLog);
    }
}
