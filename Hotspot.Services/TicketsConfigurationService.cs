using Hotspot.Model;
using Hotspot.Model.Model;
using System.Linq;
using System.Threading.Tasks;

namespace Hotspot.Services
{
    public class TicketsConfigurationService : ITicketsConfiguration
    {
        private readonly HotspotContext _context;

        public TicketsConfigurationService(HotspotContext context)
        {
            this._context = context;
        }

        public void AddFirst()
        {
            this._context.Add(new TicketsConfiguration
            {
                DefaultBandwidth = "2",
                DefaultFranchise = "2048",
                DefaultIp = "0.0.0.0",
                DefaultServer = "hotspot.com.br"
            });

            this._context.SaveChanges();
        }

        public TicketsConfiguration Get()
        {
            if(!this._context.TicketsConfiguration.Any())
            {
                //Empty
                this.AddFirst();
            }
            
            return _context.TicketsConfiguration.First();
        }

        public async Task Set(TicketsConfiguration config)
        {
            TicketsConfiguration oldConfig = Get();

            oldConfig.DefaultBandwidth = config.DefaultBandwidth;
            oldConfig.DefaultFranchise = config.DefaultFranchise;
            oldConfig.DefaultIp = config.DefaultIp;

            await _context.SaveChangesAsync();
        }
    }
}
