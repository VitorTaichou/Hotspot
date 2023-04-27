using Hotspot.Model.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hotspot.Model
{
    public interface ILocale
    {
        Task Add(Locale locale);

        Task Update(Locale locale);

        IEnumerable<Locale> GetAll();
        Task<Locale> GetById(int id);
        Task<Locale> GetByCity(string city);

        Task Remove(int id);
        Task Remove(Locale locale);
    }
}
