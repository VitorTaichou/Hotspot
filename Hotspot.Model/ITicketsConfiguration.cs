using Hotspot.Model.Model;
using System.Threading.Tasks;

namespace Hotspot.Model
{
    public interface ITicketsConfiguration
    {
        TicketsConfiguration Get();

        Task Set(TicketsConfiguration config);

        void AddFirst();
    }
}
