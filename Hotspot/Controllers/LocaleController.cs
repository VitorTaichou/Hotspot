using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hotspot.Model;
using Hotspot.Model.Model;
using Hotspot.Models.Locale;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Hotspot.Controllers
{
    public class LocaleController : Controller
    {
        private readonly ILocale _localeService;
        private readonly SignInManager<EmployeeUser> signInManager;
        private readonly ILog _logService;

        public LocaleController(ILocale localeService, SignInManager<EmployeeUser> signInManager, ILog logService)
        {
            _localeService = localeService;
            this.signInManager = signInManager;
            _logService = logService;
        }

        public IActionResult Index()
        {
            var localeList = _localeService.GetAll();
            LocaleListviewModel model = new LocaleListviewModel();
            List<LocaleViewModel> list = new List<LocaleViewModel>();

            foreach(var l in localeList)
            {
                list.Add(new LocaleViewModel()
                {
                    City = l.City,
                    Id = l.Id,
                    State = l.State,
                    Type = l.Type
                });
            }

            model.LocaleList = list;
            return View(model);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(LocaleViewModel model)
        {
            Locale newLocale = new Locale()
            {
                City = model.City,
                State = model.State,
                Type = model.Type
            };

            await _localeService.Add(newLocale);

            //Log Registration
            if (signInManager.IsSignedIn(User))
            {
                string userNameUpperCase = User.Identity.Name.First().ToString().ToUpper() + User.Identity.Name.Substring(1);
                await _logService.Add(new Log()
                {
                    User = userNameUpperCase,
                    Time = DateTime.Now,
                    Action = "Adicionou a localidade '" + model.City + ", " + model.State + "' do tipo " + model.Type
                });
            }

            return RedirectToAction("Details", "Locale", new { id = newLocale.Id });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var l = await _localeService.GetById(id);
            await _localeService.Remove(id);

            //Log Registration
            if (signInManager.IsSignedIn(User))
            {
                string userNameUpperCase = User.Identity.Name.First().ToString().ToUpper() + User.Identity.Name.Substring(1);
                await _logService.Add(new Log()
                {
                    User = userNameUpperCase,
                    Time = DateTime.Now,
                    Action = "Excluiu a localidade '" + l.City + ", " + l.State + "'"
                });
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int id)
        {
            var locale = await _localeService.GetById(id);
            LocaleViewModel model = new LocaleViewModel()
            {
                City = locale.City,
                Id = locale.Id,
                State = locale.State,
                Type = locale.Type
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var locale = await _localeService.GetById(id);
            LocaleViewModel model = new LocaleViewModel()
            {
                City = locale.City,
                Id = locale.Id,
                State = locale.State,
                Type = locale.Type
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(LocaleViewModel model)
        {
            var l = await _localeService.GetById(model.Id);

            await _localeService.Update(new Locale()
            {
                Id = model.Id,
                City = model.City,
                State = model.State,
                Type = model.Type
            });

            //Log Registration
            if (signInManager.IsSignedIn(User))
            {
                string userNameUpperCase = User.Identity.Name.First().ToString().ToUpper() + User.Identity.Name.Substring(1);
                await _logService.Add(new Log()
                {
                    User = userNameUpperCase,
                    Time = DateTime.Now,
                    Action = "Editou a localidade  de '" + l.City + ", " + l.State + ", " + l.Type + "' para '" +
                                model.City + ", " + model.State + ", " + model.Type + "'"
                });
            }

            return RedirectToAction("Details", "Locale", new { id = model.Id });
        }
    }
}
