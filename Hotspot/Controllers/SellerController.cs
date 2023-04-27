using Hotspot.Model;
using Hotspot.Model.Model;
using Hotspot.Models.Seller;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Hotspot.Controllers
{
    public class SellerController : Controller
    {
        private readonly ISeller _sellerService;
        private readonly ISale _saleService;
        private readonly ILog _logService;
        private readonly ILocale _localeService;
        private readonly SignInManager<EmployeeUser> signInManager;

        public SellerController(ISeller sellerService, ISale saleService, ILog logservice, SignInManager<EmployeeUser> signInManager, ILocale localeService)
        {
            _sellerService = sellerService;
            _saleService = saleService;
            _logService = logservice;
            _localeService = localeService;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            //List Sellers
            var sellerList = _sellerService.GetAll().Select(seller => new SellerListObject {
                Id = seller.Id,
                Cpf = seller.Cpf,
                FullName = seller.Name + " " + seller.Surname,
                Locale = seller.Address.Locale.City
            });

            var model = new SellerListViewModel();
            model.Sellers = sellerList;

            return View(model);
        }

        [HttpPost]
        public IActionResult Index(SellerListViewModel model)
        {
            if(model.Search != null)
            {
                var sellerList = _sellerService.Search(model.Search);
                List<SellerListObject> modelList = new List<SellerListObject>();

                foreach (var seller in sellerList)
                {
                    if (seller != null)
                    {
                        modelList.Add(new SellerListObject()
                        {
                            Id = seller.Id,
                            Cpf = seller.Cpf,
                            FullName = seller.Name + " " + seller.Surname
                        });
                    }
                }

                model.Sellers = modelList;
                return View(model);
            }
            else
            {
                //List Sellers
                var sellerList = _sellerService.GetAll().Select(seller => new SellerListObject
                {
                    Id = seller.Id,
                    Cpf = seller.Cpf,
                    FullName = seller.Name + " " + seller.Surname
                });

                model = new SellerListViewModel();
                model.Sellers = sellerList;

                return View(model);
            }
        }

        [HttpGet]
        public IActionResult ByCity()
        {
            var sellerList = _sellerService.GetAll();
            SellerListViewModel model = new SellerListViewModel();
            List<SellerListObject> listModel = new List<SellerListObject>();
            
            foreach(var seller in sellerList)
            {
                listModel.Add(new SellerListObject()
                {
                    Id = seller.Id,
                    Cpf = seller.Cpf,
                    FullName = seller.Name + " " + seller.Surname,
                    Locale = seller.Address.Locale.City + " " +  seller.Address.Locale.State
                });
            }

            model.Sellers = listModel;

            var locales = _localeService.GetAll();
            ViewBag.Locales = locales;

            return View(model);
        }

        [HttpPost]
        public IActionResult ByCity(SellerListViewModel model)
        {
            var sellerList = _sellerService.Search(model.Search);

            if (model.LocaleId > 0)
            {
                sellerList = sellerList.Where(s => s.Address.Locale.Id == model.LocaleId);
            }

            List<SellerListObject> listModel = new List<SellerListObject>();

            foreach (var seller in sellerList)
            {
                listModel.Add(new SellerListObject()
                {
                    Id = seller.Id,
                    Cpf = seller.Cpf,
                    FullName = seller.Name + " " + seller.Surname,
                    Locale = seller.Address.Locale.City + " " + seller.Address.Locale.State
                });
            }

            model.Sellers = listModel;

            var locales = _localeService.GetAll();
            ViewBag.Locales = locales;

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
        public async Task<IActionResult> Add(SellerViewModel model)
        {

            if(ModelState.IsValid)
            {
                //Personal Info
                Seller newSeller = new Seller();
                newSeller.Name = model.Name.ToUpper();
                newSeller.Surname = model.Surname.ToUpper();
                newSeller.Cpf = model.Cpf;
                newSeller.Rg = model.Rg;
                newSeller.Sex = model.Sex;
                newSeller.Birthday = model.Birthday;

                //Address Info
                Address newAddress = new Address();
                newAddress.Street = model.Street.ToUpper();
                newAddress.Number = model.Number.ToUpper();
                newAddress.Neighborhood = model.Neighborhood.ToUpper();
                newSeller.Address = newAddress;
                newSeller.Address.Locale = await _localeService.GetById(model.LocaleId);

                //Contact Info
                var phoneList = new List<Phonenumber>();
                if (model.PhoneNumber1 != null)
                {
                    phoneList.Add(new Phonenumber(model.PhoneNumber1));
                }
                if (model.PhoneNumber2 != null)
                {
                    phoneList.Add(new Phonenumber(model.PhoneNumber2));
                }
                newSeller.Phonenumbers = phoneList;
                newSeller.Email = model.Email.ToLower();

                await _sellerService.Create(newSeller);

                //Log Registration
                if (signInManager.IsSignedIn(User))
                {
                    string userNameUpperCase = User.Identity.Name.First().ToString().ToUpper() + User.Identity.Name.Substring(1);
                    await _logService.Add(new Log()
                    {
                        User = userNameUpperCase,
                        Time = DateTime.Now,
                        Action = "Criou o Vendedor " + newSeller.Name + " " + newSeller.Surname
                    });
                }

                return RedirectToAction("Details", "Seller", new { id = newSeller.Id });
            }
            
            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            var seller = await _sellerService.GetById(id);
            SellerViewModel sellerViewModel = new SellerViewModel(
                seller.Name, seller.Surname, seller.Email, seller.Cpf, seller.Rg,
                seller.Sex, seller.Birthday, seller.Address.Street, seller.Address.Number, 
                seller.Address.Neighborhood, seller.Address.Locale.City, seller.Address.Locale.State
            );
            sellerViewModel.Id = seller.Id;

            if (seller.Phonenumbers.Count() > 0)
            {
                sellerViewModel.PhoneNumber1 = seller.Phonenumbers.ToList()[0].Number;

                if(seller.Phonenumbers.Count() > 1)
                {
                    sellerViewModel.PhoneNumber2 = seller.Phonenumbers.ToList()[1].Number;
                }
            }

            //Making Batch List
            bool hasPendency = false;
            List<SellerBatchViewModel> batchList = new List<SellerBatchViewModel>();
            foreach(var batch in seller.Batches)
            {
                int oneHour, twoHour, threeHour, sixHour;
                oneHour = twoHour = threeHour = sixHour = 0;

                foreach(var ticket in batch.TicketList)
                {
                    if(ticket.Value == 2.0)
                    {
                        oneHour++;
                    }
                    else if (ticket.Value == 3.0)
                    {
                        twoHour++;
                    }
                    else if (ticket.Value == 5.0)
                    {
                        threeHour++;
                    }
                    else if (ticket.Value == 10.0)
                    {
                        sixHour++;
                    }
                }

                //Pendency Check
                if(!batch.Payment)
                {
                    hasPendency = true;
                }

                //Get Amount Paid
                decimal paidValue = 0;
                foreach(var flow in batch.FlowList)
                {
                    paidValue += flow.Amount;
                }

                batchList.Add(new SellerBatchViewModel()
                {
                    Id = batch.Id,
                    OneHourQty = oneHour,
                    TwoHourQty = twoHour,
                    ThreeHourQty = threeHour,
                    SixHourQty = sixHour,
                    Payment = batch.Payment,
                    PaymentMethod = batch.PaymentMethod,
                    Date = batch.Date,
                    PaidValue = paidValue,
                    TotalValue = batch.TotalValue,
                    Comission = batch.Commission
                });
            }

            List<SellerCatalogBatchViewModel> catalobBatchList = new List<SellerCatalogBatchViewModel>();
            foreach (var batch in seller.CatalogBatches)
            {
                //Pendency Check
                if (!batch.Payment)
                {
                    hasPendency = true;
                }

                //Get Amount Paid
                decimal paidValue = 0;
                foreach (var flow in batch.FlowList)
                {
                    paidValue += flow.Amount;
                }

                catalobBatchList.Add(new SellerCatalogBatchViewModel()
                {
                    Id = batch.Id,
                    Payment = batch.Payment,
                    PaymentMethod = batch.PaymentMethod,
                    Date = batch.Date,
                    PaidValue = paidValue,
                    TotalValue = batch.TotalValue,
                    Comission = batch.Commission
                });
            }

            sellerViewModel.HasPendency = hasPendency;
            sellerViewModel.Batches = batchList.OrderByDescending(b => b.Date);
            sellerViewModel.CatalogBatches = catalobBatchList.OrderByDescending(b => b.Date);
            return View(sellerViewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var seller = await _sellerService.GetById(id);
            await _sellerService.Delete(id);

            //Log Registration
            if (signInManager.IsSignedIn(User))
            {
                string userNameUpperCase = User.Identity.Name.First().ToString().ToUpper() + User.Identity.Name.Substring(1);
                await _logService.Add(new Log()
                {
                    User = userNameUpperCase,
                    Time = DateTime.Now,
                    Action = "Excluiu o Vendedor " + seller.Name + " " + seller.Surname + " e todos os Lotes e registros de Vendas deste vendedor foram excluídos."
                });
            }

            return RedirectToAction("Index", "Seller");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var seller = await _sellerService.GetById(id);
            SellerViewModel sellerViewModel = new SellerViewModel(
                seller.Name.ToUpper(), seller.Surname.ToUpper(), seller.Email.ToLower(), seller.Cpf, seller.Rg,
                seller.Sex, seller.Birthday, seller.Address.Street.ToUpper(), seller.Address.Number,
                seller.Address.Neighborhood.ToUpper(), seller.Address.Locale.City, seller.Address.Locale.State
            );
            sellerViewModel.Id = seller.Id;
            sellerViewModel.LocaleId = seller.Address.Locale.Id;

            if (seller.Phonenumbers.Count() > 0)
            {
                sellerViewModel.PhoneNumber1 = seller.Phonenumbers.ToList()[0].Number;

                if (seller.Phonenumbers.Count() > 1)
                {
                    sellerViewModel.PhoneNumber2 = seller.Phonenumbers.ToList()[1].Number;
                }
            }

            //Log Registration
            if (signInManager.IsSignedIn(User))
            {
                string userNameUpperCase = User.Identity.Name.First().ToString().ToUpper() + User.Identity.Name.Substring(1);
                await _logService.Add(new Log()
                {
                    User = userNameUpperCase,
                    Time = DateTime.Now,
                    Action = "Excluiu o Vendedor " + seller.Name + " " + seller.Surname + " e todos os Lotes e Histórico de Vendas deste vendedor foram excluídos"
                });
            }

            var locales = _localeService.GetAll();
            ViewBag.Locales = locales;

            return View(sellerViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SellerViewModel sellerViewModel)
        {
            var locale = await _localeService.GetById(sellerViewModel.LocaleId);
            if (ModelState.IsValid)
            {
                //Personal Info
                Seller newSeller = new Seller();
                newSeller.Name = sellerViewModel.Name.ToUpper();
                newSeller.Surname = sellerViewModel.Surname.ToUpper();
                newSeller.Cpf = sellerViewModel.Cpf;
                newSeller.Rg = sellerViewModel.Rg;
                newSeller.Sex = sellerViewModel.Sex;
                newSeller.Birthday = sellerViewModel.Birthday;

                //Address Info
                Address newAddress = new Address();
                newAddress.Street = sellerViewModel.Street.ToUpper();
                newAddress.Number = sellerViewModel.Number.ToUpper();
                newAddress.Neighborhood = sellerViewModel.Neighborhood.ToUpper();
                newAddress.Locale = new Locale();
                newAddress.Locale.City = locale.City;
                newAddress.Locale.State = locale.State;
                newSeller.Address = newAddress;

                //Contact Info
                var phoneList = new List<Phonenumber>();
                if (sellerViewModel.PhoneNumber1 != null)
                {
                    phoneList.Add(new Phonenumber(sellerViewModel.PhoneNumber1));
                }
                if (sellerViewModel.PhoneNumber2 != null)
                {
                    phoneList.Add(new Phonenumber(sellerViewModel.PhoneNumber2));
                }
                newSeller.Id = sellerViewModel.Id;
                newSeller.Phonenumbers = phoneList;
                newSeller.Email = sellerViewModel.Email.ToLower();

                await _sellerService.Edit(newSeller);

                //Log Registration
                if (signInManager.IsSignedIn(User))
                {
                    string userNameUpperCase = User.Identity.Name.First().ToString().ToUpper() + User.Identity.Name.Substring(1);
                    await _logService.Add(new Log()
                    {
                        User = userNameUpperCase,
                        Time = DateTime.Now,
                        Action = "Editou o Vendedor de ID " + sellerViewModel.Id
                    });
                }

                return RedirectToAction("Details", "Seller", new { id = sellerViewModel.Id });
            }

            return RedirectToAction("Details", "Seller", new { id = sellerViewModel.Id });
        }
    }
}