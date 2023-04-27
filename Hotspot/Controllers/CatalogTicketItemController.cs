using Hotspot.Model;
using Hotspot.Models.CatalogTicket;
using Hotspot.Tools.Mikrotik;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Hotspot.Controllers
{
    public class CatalogTicketItemController : Controller
    {
        private readonly ICatalogTicketItem _catalogTicketItemService;
        private readonly ITicketsConfiguration _ticketsConfiguration;

        public CatalogTicketItemController(ICatalogTicketItem catalogTicketItemService, ITicketsConfiguration ticketsConfiguration)
        {
            _catalogTicketItemService = catalogTicketItemService;
            _ticketsConfiguration = ticketsConfiguration;
        }

        public IActionResult Index()
        {
            CatalogTicketItemListViewModel model = new CatalogTicketItemListViewModel();
            List<CatalogTicketItemViewModel> modelList = new List<CatalogTicketItemViewModel>();
            var list = _catalogTicketItemService.GetAll();

            foreach (var item in list)
            {
                modelList.Add(new CatalogTicketItemViewModel()
                {
                    Bandwidth = item.Bandwidth,
                    Color = item.Color,
                    Franchise = item.Franchise,
                    Id = item.Id,
                    Time = TimeSpan.FromSeconds(item.Time),
                    Value = item.Value.ToString(),
                    Description = item.Description,
                    ExpireDays = item.ExpireDays,
                    Quantity = item.ExpireDays
                });
            }
            model.ItemsList = modelList;

            return View(model);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(CatalogTicketItemViewModel model)
        {
            //Region info
            var style = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands;
            var provider = new CultureInfo("pt-BR");
            if (model.Value.Contains("."))
            {
                provider = new CultureInfo("en-US");
            }

            //Get Amount
            decimal amount = decimal.Parse(model.Value, style, provider);

            await _catalogTicketItemService.Create(new Model.Model.CatalogTicketItem()
            {
                Bandwidth = model.Bandwidth,
                Color = model.Color,
                Franchise = model.Franchise,
                Time = (long) model.Time.Add(TimeSpan.FromDays(model.TimeDays)).TotalSeconds,
                Value = amount,
                ExpireDays = model.ExpireDays,
                Description = model.Description
            });

            //Create profile
            string profileName = model.Description.Replace(" ", string.Empty);
            profileName = Regex.Replace(profileName, @"[^\u0020-\u007E]", string.Empty);
            var config = _ticketsConfiguration.Get();

            MikrotikHandler mikrotikHandler = new MikrotikHandler();
            await mikrotikHandler.CreateProfile(int.Parse(model.Bandwidth), _ticketsConfiguration.Get().DefaultIp, profileName, config.DefaultServer);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var old = _catalogTicketItemService.Get(id);

            MikrotikHandler mikrotikHandler = new MikrotikHandler();

            string profileName = old.Description.Replace(" ", string.Empty);
            profileName = Regex.Replace(profileName, @"[^\u0020-\u007E]", string.Empty);

            //Delete
            await mikrotikHandler.DeleteProfile(_ticketsConfiguration.Get().DefaultIp, profileName);

            _catalogTicketItemService.Remove(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var item = _catalogTicketItemService.Get(id);
            CatalogTicketItemViewModel model = new CatalogTicketItemViewModel()
            {
                Bandwidth = item.Bandwidth,
                Color = item.Color,
                Franchise = item.Franchise,
                Id = item.Id,
                Value = item.Value.ToString(),
                Description = item.Description,
                ExpireDays = item.ExpireDays,
                Quantity = item.ExpireDays
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CatalogTicketItemViewModel model)
        {
            var old = _catalogTicketItemService.Get(model.Id);

            MikrotikHandler mikrotikHandler = new MikrotikHandler();

            string oldProfileName = old.Description.Replace(" ", string.Empty);
            oldProfileName = Regex.Replace(oldProfileName, @"[^\u0020-\u007E]", string.Empty);

            string profileName = model.Description.Replace(" ", string.Empty);
            profileName = Regex.Replace(profileName, @"[^\u0020-\u007E]", string.Empty);

            //Delete
            await mikrotikHandler.DeleteProfile(_ticketsConfiguration.Get().DefaultIp, oldProfileName);

            await _catalogTicketItemService.Update(new Model.Model.CatalogTicketItem()
            {
                Bandwidth = model.Bandwidth,
                Id = model.Id,
                Color = model.Color,
                Description = model.Description,
                ExpireDays = model.ExpireDays,
                Franchise = model.Franchise,
                Time = (long) model.Time.Add(TimeSpan.FromDays(model.TimeDays)).TotalSeconds,
                Value = decimal.Parse(model.Value)
            });

            //Create profile
            var config = _ticketsConfiguration.Get();
            await mikrotikHandler.CreateProfile(int.Parse(model.Bandwidth), _ticketsConfiguration.Get().DefaultIp, profileName, config.DefaultServer);

            return RedirectToAction("Index");
        }
    }
}
