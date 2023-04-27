using Hotspot.Model.Model;
using System.Threading.Tasks;

namespace Hotspot.Model
{
    public interface IEmployeeUser
    {
        Task<EmployeeUser> GetById(string id);
    }
}
