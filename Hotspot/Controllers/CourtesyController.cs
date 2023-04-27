using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Hotspot.Model;
using Hotspot.Model.Model;
using Hotspot.Models.Courtesy;
using Hotspot.Tools.Mikrotik;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Hotspot.Controllers
{
    public class CourtesyController : Controller
    {
        private readonly ICourtesy _courtesyService;
        private readonly ILog _logService;
        private readonly ITicketsConfiguration _ticketsConfiguration;
        private readonly SignInManager<EmployeeUser> signInManager;

        public CourtesyController(ICourtesy courtesyService, ILog logService, ITicketsConfiguration ticketsConfiguration, SignInManager<EmployeeUser> signInManager)
        {
            _courtesyService = courtesyService;
            _logService = logService;
            _ticketsConfiguration = ticketsConfiguration;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            CourtesyListViewModel model = new CourtesyListViewModel();
            List<CourtesyViewModel> courtesyList = new List<CourtesyViewModel>();
            var courtesies = _courtesyService.GetAll();

            foreach(var courtesy in courtesies)
            {
                courtesyList.Add(new CourtesyViewModel()
                {
                    Code = courtesy.Code,
                    Description = courtesy.Description,
                    FullName = courtesy.Name + " " + courtesy.Surname,
                    Id = courtesy.Id
                });
            }

            model.Courtesies = courtesyList;
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(CourtesyListViewModel model)
        {
            if(model.Search != null)
            {
                var courtesySearchList = _courtesyService.Search(model.Search);
                List<CourtesyViewModel> modelList = new List<CourtesyViewModel>();

                foreach (var courtesy in courtesySearchList)
                {
                    if (courtesy != null)
                    {
                        modelList.Add(new CourtesyViewModel()
                        {
                            Code = courtesy.Code,
                            Description = courtesy.Description,
                            FullName = courtesy.Name + " " + courtesy.Surname,
                            Id = courtesy.Id
                        });
                    }
                }

                model.Courtesies = modelList;
                return View(model);
            }
            else
            {
                List<CourtesyViewModel> courtesyList = new List<CourtesyViewModel>();
                var courtesies = _courtesyService.GetAll();

                foreach (var courtesy in courtesies)
                {
                    courtesyList.Add(new CourtesyViewModel()
                    {
                        Code = courtesy.Code,
                        Description = courtesy.Description,
                        FullName = courtesy.Name + " " + courtesy.Surname,
                        Id = courtesy.Id
                    });
                }

                model.Courtesies = courtesyList;
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(CourtesyAddViewModel newCourtesy)
        {
            Courtesy courtesy = new Courtesy()
            {
                Code = newCourtesy.Code,
                Description = newCourtesy.Description,
                Name = newCourtesy.Name.ToUpper(),
                Surname = newCourtesy.Surname.ToUpper()
            };

            await _courtesyService.Add(courtesy);

            //Log Registration
            if (signInManager.IsSignedIn(User))
            {
                string userNameUpperCase = User.Identity.Name.First().ToString().ToUpper() + User.Identity.Name.Substring(1);
                await _logService.Add(new Log()
                {
                    User = userNameUpperCase,
                    Time = DateTime.Now,
                    Action = "Criou a cortesia " + newCourtesy.Name + " " + newCourtesy.Surname
                });
            }

            return RedirectToAction("Details", "Courtesy", new { id = courtesy.Id });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var courtesy = await _courtesyService.GetById(id);

            //try to Disconnect the deleted
            MikrotikHandler mkHandler = new MikrotikHandler();
            await mkHandler.DeleteActiveConnection(_ticketsConfiguration.Get().DefaultIp, courtesy.Code);

            await _courtesyService.Delete(id);

            //Log Registration
            if (signInManager.IsSignedIn(User))
            {
                string userNameUpperCase = User.Identity.Name.First().ToString().ToUpper() + User.Identity.Name.Substring(1);
                await _logService.Add(new Log()
                {
                    User = userNameUpperCase,
                    Time = DateTime.Now,
                    Action = "Excluiu a Cortesia de " + courtesy.Name + " " + courtesy.Surname
                });
            }


            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int id)
        {
            var courtesy = await _courtesyService.GetById(id);
            CourtesyViewModel model = new CourtesyViewModel()
            {
                Code = courtesy.Code,
                Description = courtesy.Description,
                FullName = courtesy.Name + " " + courtesy.Surname,
                Id = courtesy.Id
            };
            
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var courtesy = await _courtesyService.GetById(id);

            CourtesyAddViewModel courtesyModel = new CourtesyAddViewModel()
            {
                Code = courtesy.Code,
                Description = courtesy.Description,
                Id = courtesy.Id,
                Name = courtesy.Name.ToUpper(),
                Surname = courtesy.Surname.ToUpper()
            };

            return View(courtesyModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CourtesyAddViewModel model)
        {
            Courtesy courtesyToEdit = new Courtesy()
            {
                Description = model.Description,
                Name = model.Name.ToUpper(),
                Surname = model.Surname.ToUpper(),
                Id = model.Id
            };

            await _courtesyService.Edit(courtesyToEdit);

            //Log Registration
            if (signInManager.IsSignedIn(User))
            {
                string userNameUpperCase = User.Identity.Name.First().ToString().ToUpper() + User.Identity.Name.Substring(1);
                await _logService.Add(new Log()
                {
                    User = userNameUpperCase,
                    Time = DateTime.Now,
                    Action = "Editou a Cortesia " + model.Id
                }) ;
            }

            return RedirectToAction("Details", "Courtesy", new { id = model.Id });
        }
    }
}