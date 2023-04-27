using Hotspot.Model;
using Hotspot.Model.Model;
using Hotspot.Models.CashFlow;
using Hotspot.Models.Flow;
using Hotspot.Models.Locale;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Hotspot.Controllers
{
    public class CashFlowController : Controller
    {
        private readonly ICashFlow _cashFlowService;
        private readonly ILocale _localeService;
        private readonly UserManager<EmployeeUser> userManager;
        private readonly SignInManager<EmployeeUser> signInManager;
        private readonly ILog _logService;

        public CashFlowController(ICashFlow cashFlowService, ILocale localeService, UserManager<EmployeeUser> userManager, SignInManager<EmployeeUser> signInManager, ILog logService)
        {
            _cashFlowService = cashFlowService;
            _localeService = localeService;
            this.userManager = userManager;
            this.signInManager = signInManager;
            _logService = logService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            //Show Cashflow List
            CashFlowListViewModel model = new CashFlowListViewModel();
            List<CashFlowViewModel> list = new List<CashFlowViewModel>();

            var cashFlowList = _cashFlowService.GetAll();
            foreach(var cf in cashFlowList)
            {
                list.Add(new CashFlowViewModel()
                {
                    Id = cf.Id,
                    Name = cf.Name,
                    LocaleViewModel = new LocaleViewModel()
                    {
                        Id = cf.Locale.Id,
                        City = cf.Locale.City,
                        State = cf.Locale.State,
                        Type = cf.Locale.Type
                    },
                    FlowViewModelList = null
                });
            }
            model.CashFlowList = list;

            return View(model);
        }

        [HttpGet]
        public IActionResult Add()
        {
            var locales = _localeService.GetAll();
            ViewBag.Locales = locales;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(CashFlowViewModel model)
        {
            var locale = await _localeService.GetById(model.LocaleId);
            CashFlow cashFlow = new CashFlow()
            {
                Name = model.Name,
                Locale = locale
            };

            await _cashFlowService.Add(cashFlow);

            //Log Registration
            if (signInManager.IsSignedIn(User))
            {
                string userNameUpperCase = User.Identity.Name.First().ToString().ToUpper() + User.Identity.Name.Substring(1);
                await _logService.Add(new Log()
                {
                    User = userNameUpperCase,
                    Time = DateTime.Now,
                    Action = "Adicionou o Caixa " + cashFlow.Name
                });
            }

            return RedirectToAction("Details", "CashFlow", new { id = 1});
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var cashFlow = await _cashFlowService.GetById(id);
            decimal totalAmount = 0;

            CashFlowViewModel model = new CashFlowViewModel()
            {
                Id = cashFlow.Id,
                LocaleId = cashFlow.Locale.Id,
                Name = cashFlow.Name,
                
            };
            List<FlowViewModel> flowList = new List<FlowViewModel>();

            //Seed flowList
            foreach(var flow in cashFlow.Flows)
            {
                FlowViewModel f = new FlowViewModel()
                {
                    Amount = flow.Amount.ToString("C2"),
                    CashFlow = model,
                    Description = flow.Description,
                    Id = flow.Id,
                    Date = flow.Date
                };

                if(flow.EmployeeUser != null)
                {
                    f.EmployeeName = flow.EmployeeUser.Name;
                }

                if (flow.Amount >= 0)
                {
                    f.Type = "INFLOW";
                    model.TotalInflow += flow.Amount;
                }
                else
                {
                    f.Type = "OUTFLOW";
                    model.TotalOutflow += flow.Amount;
                }

                totalAmount += flow.Amount;
                flowList.Add(f);
            }
            model.FlowViewModelList = flowList;

            //Locale
            model.LocaleViewModel = new LocaleViewModel()
            {
                City = cashFlow.Locale.City,
                Id = cashFlow.Locale.Id,
                State = cashFlow.Locale.State,
                Type = cashFlow.Locale.Type
            };

            model.FlowViewModelList = model.FlowViewModelList.OrderByDescending(l => l.Date).ToList();
            model.TotalAmount = totalAmount;
            model.StartDate = model.EndDate = DateTime.Now;
            model.CurrentAmount = totalAmount;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Details(CashFlowViewModel model)
        {
            var cashFlow = await _cashFlowService.GetById(model.Id);

            decimal totalAmount = 0;

            foreach(var flow in cashFlow.Flows)
            {
                totalAmount += flow.Amount;
            }

            DateTime start = model.StartDate;
            DateTime end = model.EndDate + new TimeSpan(23, 59, 59);

            var searchFlowList = _cashFlowService.GetFlowByDate(model.Id, start, end);
            List<FlowViewModel> flowList = new List<FlowViewModel>();

            CashFlowViewModel newModel = new CashFlowViewModel()
            {
                Id = cashFlow.Id,
                LocaleId = cashFlow.Locale.Id,
                Name = cashFlow.Name,
            };

            decimal inflow = 0;
            decimal outflow = 0;

            //Seed flowList
            foreach (var flow in searchFlowList)
            {
                FlowViewModel f = new FlowViewModel()
                {
                    Amount = flow.Amount.ToString("C2"),
                    CashFlow = newModel,
                    Description = flow.Description,
                    Id = flow.Id,
                    Date = flow.Date
                };

                if (flow.Amount > 0)
                {
                    f.Type = "INFLOW";
                    inflow += flow.Amount;
                }
                else
                {
                    f.Type = "OUTFLOW";
                    outflow += flow.Amount;
                }

                flowList.Add(f);
            }
            newModel.FlowViewModelList = flowList;

            //Locale
            newModel.LocaleViewModel = new LocaleViewModel()
            {
                City = cashFlow.Locale.City,
                Id = cashFlow.Locale.Id,
                State = cashFlow.Locale.State,
                Type = cashFlow.Locale.Type
            };

            newModel.FlowViewModelList = newModel.FlowViewModelList.OrderByDescending(l => l.Date).ToList();
            newModel.TotalAmount = totalAmount;
            newModel.TotalInflow = inflow;
            newModel.TotalOutflow = outflow;
            newModel.CurrentAmount = inflow + outflow;

            return View(newModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var cf = await _cashFlowService.GetById(id);

            //Log Registration
            if (signInManager.IsSignedIn(User))
            {
                string userNameUpperCase = User.Identity.Name.First().ToString().ToUpper() + User.Identity.Name.Substring(1);
                await _logService.Add(new Log()
                {
                    User = userNameUpperCase,
                    Time = DateTime.Now,
                    Action = "Removeu o Caixa " + cf.Name
                });
            }

            await _cashFlowService.Delete(id);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AddFlow(int id)
        {
            FlowViewModel model = new FlowViewModel()
            {
                CashFlowId = id
            };

            DateTime date = DateTime.Now;
            model.Date = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, date.Kind);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddFlow(FlowViewModel model)
        {
            var style = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands;
            var cf = await _cashFlowService.GetById(model.CashFlowId);
            var provider = new CultureInfo("pt-BR");

            if (model.Amount.Contains("."))
            {
                provider = new CultureInfo("en-US");
            }

            decimal amount = decimal.Parse(model.Amount, style, provider);

            if (model.Type.Equals("OUTFLOW"))
            {
                amount *= -1;
            }

            await _cashFlowService.AddFlow(model.CashFlowId, new Flow()
            {
                Amount = amount,
                Date = model.Date,
                Description = model.Description,
                EmployeeUser = await userManager.FindByIdAsync(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value)
            });

            //Log Registration
            if (signInManager.IsSignedIn(User))
            {
                string userNameUpperCase = User.Identity.Name.First().ToString().ToUpper() + User.Identity.Name.Substring(1);
                await _logService.Add(new Log()
                {
                    User = userNameUpperCase,
                    Time = DateTime.Now,
                    Action = "Adicionou a quantia de " + amount + " no caixa " + cf.Name
                });
            }

            return RedirectToAction("Details", "CashFlow", new { id = model.CashFlowId});
        }

        public async Task<IActionResult> DeleteFlow(string id)
        {
            var flow = await _cashFlowService.GetFlowById(id);
            await _cashFlowService.DeleteFlow(id);

            //Log Registration
            if (signInManager.IsSignedIn(User))
            {
                string userNameUpperCase = User.Identity.Name.First().ToString().ToUpper() + User.Identity.Name.Substring(1);
                await _logService.Add(new Log()
                {
                    User = userNameUpperCase,
                    Time = DateTime.Now,
                    Action = "Removeu o registro de " + flow.Amount + " do caixa " + flow.CashFlow.Name
                });
            }

            return RedirectToAction("Details", "CashFlow", new { id = flow.CashFlow.Id });
        }
    }
}
