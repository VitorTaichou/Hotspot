using Hotspot.Model;
using Hotspot.Model.Model;
using Hotspot.Models.CatalogTicket;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Hotspot.Controllers
{
    public class CatalogBatchController : Controller
    {
        public readonly ICatalogTicketItem _calatogTicketItemService;
        public readonly ICatalogBatch _catalogBatchService;
        private readonly ISeller _sellerService;
        private readonly IEmployeeUser _employeeUserService;
        private readonly UserManager<EmployeeUser> userManager;
        private readonly ILog _logService;
        private readonly ILocale _localeService;
        private readonly ICashFlow _cashFlowService;
        private readonly SignInManager<EmployeeUser> signInManager;

        public CatalogBatchController(ICatalogTicketItem calatogTicketItemService, 
            ICatalogBatch catalogBatchService, ISeller sellerService, IEmployeeUser employeeUserService, 
            UserManager<EmployeeUser> userManager, ILog logService, ILocale localeService, ICashFlow cashFlowService, 
            SignInManager<EmployeeUser> signInManager)
        {
            _calatogTicketItemService = calatogTicketItemService;
            _catalogBatchService = catalogBatchService;
            _sellerService = sellerService;
            _employeeUserService = employeeUserService;
            this.userManager = userManager;
            _logService = logService;
            _localeService = localeService;
            _cashFlowService = cashFlowService;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Create(int id)
        {
            var ticketList = _calatogTicketItemService.GetAll();
            CatalogTicketItemListViewModel model = new CatalogTicketItemListViewModel();
            List<CatalogTicketItemViewModel> list = new List<CatalogTicketItemViewModel>();

            foreach(var item in ticketList)
            {
                list.Add(new CatalogTicketItemViewModel()
                {
                    Bandwidth = item.Bandwidth,
                    Color = item.Color,
                    Franchise = item.Franchise,
                    Id = item.Id,
                    Time = TimeSpan.FromSeconds(item.Time),
                    Value = item.Value.ToString(),
                    ExpireDays = item.ExpireDays,
                    Description = item.Description
                });
            }

            model.ItemsList = list;
            model.SellerId = id;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CatalogTicketItemListViewModel model)
        {
            var seller = await _sellerService.GetById(model.SellerId);
            var cashFlowList = _cashFlowService.GetAll();
            var cf = cashFlowList.Where(c => c.Locale == seller.Address.Locale).First();

            //Generate Batch
            var newBatch = new CatalogBatch()
            {
                Commission = model.Comission,
                Date = DateTime.Now,
                CashFlow = cf,
                EmployeeUser = await userManager.FindByIdAsync(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value),
                PaymentMethod = model.PaymentMethod,
                Seller = seller
            };
            //Pending Payment

            //Creating Ticket List and getting Total Value and discount
            List<CatalogTicket> ticketList = new List<CatalogTicket>();
            decimal totalValue = 0;

            foreach(var catalogItem in model.ItemsList)
            {
                 var catalogItemRecovery = _calatogTicketItemService.Get(catalogItem.Id);
                for (int i = 0; i < catalogItem.Quantity; i++)
                {
                    //Creating Tickets
                    ticketList.Add(new CatalogTicket()
                    {
                        Bandwidth = catalogItemRecovery.Bandwidth,
                        Batch = newBatch,
                        Color = catalogItemRecovery.Color,
                        Franchise = catalogItemRecovery.Franchise * 1048576,
                        Time = catalogItemRecovery.Time,
                        Value = catalogItemRecovery.Value,
                        ExpirationDays = catalogItemRecovery.ExpireDays,
                        ProfileName = Regex.Replace(catalogItemRecovery.Description.Replace(" ", String.Empty), @"[^\u0020-\u007E]", string.Empty)
                        //Id and Code to be created @ CatalogBatchService
                    });
                }
                totalValue += catalogItemRecovery.Value * catalogItem.Quantity;
            }
            decimal percentage = (decimal) model.Comission / (decimal) 100;
            decimal discount = totalValue * percentage;
            newBatch.TotalValue = totalValue - discount;
            //decimal discount = total * (model.Commission / 100);

            newBatch.TicketList = ticketList;
            await _catalogBatchService.Create(newBatch);

            //CashFlow setup
            if (newBatch.PaymentMethod == "A VISTA")
            {
                newBatch.Payment = true;
                await _cashFlowService.AddFlow(newBatch.CashFlow, new Flow()
                {
                    CatalogBatch = newBatch,
                    Amount = totalValue - discount,
                    CashFlow = newBatch.CashFlow,
                    Date = DateTime.Now,
                    EmployeeUser = newBatch.EmployeeUser,
                    Description = "Venda de Lote ao Vendedor " + newBatch.Seller.Name + " " + newBatch.Seller.Surname + "\r\n"
                    + "[Lote Criado Via Catálogo]"
                });
            }
            else
            {
                newBatch.Payment = false;
            }

            //Log Registration
            if (signInManager.IsSignedIn(User))
            {
                string userNameUpperCase = User.Identity.Name.First().ToString().ToUpper() + User.Identity.Name.Substring(1);
                await _logService.Add(new Log()
                {
                    User = userNameUpperCase,
                    Time = DateTime.Now,
                    Action = "Criou o Lote " + newBatch.Id + "[Via Catálogo]"
                });
            }

            //Redirect to seller page
            return RedirectToAction("Details", "Seller", new { id = seller.Id });
        }

        public async Task<IActionResult> Print(int id)
        {
            CatalogTicketItemListViewModel model = new CatalogTicketItemListViewModel();
            var batch = await _catalogBatchService.GetById(id);
            List<CatalogTicketItemViewModel> list = new List<CatalogTicketItemViewModel>();

            foreach(var item in batch.TicketList)
            {
                list.Add(new CatalogTicketItemViewModel()
                {
                    Bandwidth = item.Bandwidth,
                    Color = item.Color,
                    ExpireDays = item.ExpirationDays,
                    Franchise = item.Franchise,
                    Id = item.Id,
                    Time = TimeSpan.FromSeconds(item.Time),
                    Value = "R$ " + @String.Format("{0:0.00}", item.Value),
                    Code = item.Code
                });
            }
            model.ItemsList = list.OrderBy(i => i.Value).ToList();
            model.SellerCity = batch.Seller.Address.Locale.City;
            model.SellerName = batch.Seller.Name + " " + batch.Seller.Surname;


            return View(model);
        }

        public async Task<IActionResult> Delete(int id, int sellerid)
        {
            await _catalogBatchService.Delete(id);


            return RedirectToAction("Details", "Seller", new { id = sellerid });
        }
    }
}
