using Hotspot.Model;
using Hotspot.Models.Configuration;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Hotspot.Controllers
{
    public class TicketsConfigurationController : Controller
    {
        private readonly ITicketsConfiguration _ticketConfigurationService;

        public TicketsConfigurationController(ITicketsConfiguration ticketConfigurationService)
        {
            _ticketConfigurationService = ticketConfigurationService;
        }

        public IActionResult Index()
        {
            var config = _ticketConfigurationService.Get();

            TicketsConfigurationViewModel model = new TicketsConfigurationViewModel()
            {
                DefaultBandwidth = config.DefaultBandwidth,
                DefaultFranchise = config.DefaultFranchise,
                DefaultIp = config.DefaultIp,
                DefaultServer = config.DefaultServer
            };
            
            return View(model);
        }

        public async Task<IActionResult> Set(TicketsConfigurationViewModel model)
        {
            await _ticketConfigurationService.Set(new Model.Model.TicketsConfiguration
            {
                DefaultBandwidth = model.DefaultBandwidth,
                DefaultFranchise = model.DefaultFranchise,
                DefaultIp = model.DefaultIp,
                DefaultServer = model.DefaultServer
            });

            return RedirectToAction("Index");
        }
    }
}
