using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Hotspot.Model;
using Hotspot.Model.Model;
using Hotspot.Models.Batch;
using Hotspot.Tools.Code;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Hotspot.Controllers
{
    public class BatchController : Controller
    {
        private readonly ITicketsConfiguration _ticketsConfigurationService;
        private readonly IBatch _batchService;
        private readonly ISeller _sellerService;
        private readonly ISale _saleService;
        private readonly IEmployeeUser _employeeUserService;
        private readonly IGenerator _generatorService;
        private readonly UserManager<EmployeeUser> userManager;
        private readonly ILog _logService;
        private readonly ILocale _localeService;
        private readonly ICashFlow _cashFlowService;
        private readonly SignInManager<EmployeeUser> signInManager;

        public BatchController(IBatch batchService, ISeller sellerService, ISale saleService, 
            IEmployeeUser employeeUserService, IGenerator generatorService, 
            UserManager<EmployeeUser> userManager, ILog logservice, SignInManager<EmployeeUser> signInManager, 
            ICashFlow cashFlowService, ITicketsConfiguration ticketsConfigurationService)
        {
            _batchService = batchService;
            _sellerService = sellerService;
            _saleService = saleService;
            _employeeUserService = employeeUserService;
            _generatorService = generatorService;
            this.userManager = userManager;
            _logService = logservice;
            this.signInManager = signInManager;
            _cashFlowService = cashFlowService;
            _ticketsConfigurationService = ticketsConfigurationService;
        }

        [HttpGet]
        public async Task<IActionResult> Add(int id)
        {
            BatchCreateViewModel model = new BatchCreateViewModel(id);

            var seller = await _sellerService.GetById(id);
            var cashFlowList = _cashFlowService.GetAll();
            var cf = cashFlowList.Where(c => c.Locale == seller.Address.Locale).First();
            model.CashFlowId = cf.Id;

            ViewBag.SellerName = seller.Name + " " + seller.Surname;
            ViewBag.CashFlowList = _cashFlowService.GetAll();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(BatchCreateViewModel model)
        {
            //Calculating Total value and discount
            decimal total = (model.OneHourQty * 2) + (model.TwoHourQty * 3) + (model.ThreeHourQty * 5) + (model.SixHourQty * 10);
            decimal discount = total * (model.Commission / 100);

            Batch newBatch = new Batch()
            {
                EmployeeUser = await userManager.FindByIdAsync(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value),
                Seller = await _sellerService.GetById(model.SellerId),
                PaymentMethod = model.PaymentMethod,
                Commission = model.Commission,
                OneHourQty = model.OneHourQty,
                TwoHourQty = model.TwoHourQty,
                ThreeHourQty = model.ThreeHourQty,
                SixHourQty = model.SixHourQty,
                CashFlow = await _cashFlowService.GetById(model.CashFlowId),
                Date = DateTime.Now,
                TotalValue = total - discount,
            };
            List<Ticket> ticketList = new List<Ticket>();

            //Get Default Settings
            var ticketsConfig = _ticketsConfigurationService.Get();

            //Check quantities
            //One Hour
            if (model.OneHourQty > 0)
            {
                for (int i = 0; i < model.OneHourQty; i++)
                {
                    ticketList.Add(new Ticket
                    {
                        Value = 2.0,
                        Time = TimeSpan.FromHours(1),
                        Batch = newBatch,
                        Bandwidth = ticketsConfig.DefaultBandwidth,
                        Franchise = long.Parse(ticketsConfig.DefaultFranchise) * 1048576
                    });
                }
            }

            //Two Hour
            if (model.TwoHourQty > 0)
            {
                for (int i = 0; i < model.TwoHourQty; i++)
                {
                    ticketList.Add(new Ticket
                    {
                        Value = 3.0,
                        Time = TimeSpan.FromHours(2),
                        Batch = newBatch,
                        Bandwidth = ticketsConfig.DefaultBandwidth,
                        Franchise = long.Parse(ticketsConfig.DefaultFranchise) * 1048576
                    });
                }
            }

            //Three Hour
            if (model.ThreeHourQty > 0)
            {
                for (int i = 0; i < model.ThreeHourQty; i++)
                {
                    ticketList.Add(new Ticket
                    {
                        Value = 5.0,
                        Time = TimeSpan.FromHours(3),
                        Batch = newBatch,
                        Bandwidth = ticketsConfig.DefaultBandwidth,
                        Franchise = long.Parse(ticketsConfig.DefaultFranchise) * 1048576
                    });
                }
            }

            //Six Hour
            if (model.SixHourQty > 0)
            {
                for (int i = 0; i < model.SixHourQty; i++)
                {
                    ticketList.Add(new Ticket
                    {
                        Value = 10.0,
                        Time = TimeSpan.FromHours(6),
                        Batch = newBatch,
                        Bandwidth = ticketsConfig.DefaultBandwidth,
                        Franchise = long.Parse(ticketsConfig.DefaultFranchise) * 1048576
                    });
                }
            }

            //Adding Ticket List
            newBatch.TicketList = ticketList;

            //Save Batch
            await _batchService.Create(newBatch);

            //Creating Flow
            if (model.PaymentMethod.Equals("A VISTA"))
            {
                newBatch.Payment = true;
                await _cashFlowService.AddFlow(newBatch.CashFlow, new Flow()
                {
                    Batch = newBatch,
                    Amount = total - discount,
                    CashFlow = newBatch.CashFlow,
                    Date = DateTime.Now,
                    EmployeeUser = newBatch.EmployeeUser,
                    Description = "Venda de Lote ao Vendedor " + newBatch.Seller.Name + " " + newBatch.Seller.Surname + "\r\n"
                    + "[R$ 2,00 " + model.OneHourQty + ", R$ 3,00 " + model.TwoHourQty + ", R$ 5,00 "
                    + model.ThreeHourQty + ", R$ 10,00 " + model.SixHourQty + "]"
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
                    Action = "Criou o Lote " + newBatch.Id
                });
            }

            return RedirectToAction("Details", "Seller", new { id = model.SellerId});
        }

        public async Task<IActionResult> Delete(int batch, int seller)
        {
            await _batchService.Delete(batch);

            //Log Registration
            if (signInManager.IsSignedIn(User))
            {
                string userNameUpperCase = User.Identity.Name.First().ToString().ToUpper() + User.Identity.Name.Substring(1);
                await _logService.Add(new Log()
                {
                    User = userNameUpperCase,
                    Time = DateTime.Now,
                    Action = "Excluiu o Lote " + batch
                });
            }

            return RedirectToAction("Details", "Seller", new { id = seller });
        }

        public async Task<IActionResult> Print(int id)
        {
            var batch = await _batchService.GetById(id);
            //var sale = await _saleService.GetByBatchId(id);
            BatchTicketListViewModel model = new BatchTicketListViewModel();
            List<BatchTicketViewModel> ticketList = new List<BatchTicketViewModel>();

            foreach (var ticket in batch.TicketList)
            {
                if(ticket != null)
                {
                    ticketList.Add(new BatchTicketViewModel()
                    {
                        Code = ticket.Code,
                        DueDate = batch.Date.AddDays(30),
                        SellerCity = batch.Seller.Address.Locale.City,
                        SellerName = batch.Seller.Name + " " + batch.Seller.Surname,
                        TimeLeft = ticket.Time,
                        Value = "R$ " + @String.Format("{0:0.00}", ticket.Value)
                    });
                }
            }

            model.TicketList = ticketList.OrderBy(o => o.TimeLeft).ToList();

            string name = batch.Seller.Name + " " + batch.Seller.Surname;
            string date = batch.Date.ToString("ddMMyyyy-HHmmss");
            name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name.ToLower());
            name = name.Replace(" ", string.Empty);
            ViewBag.PageTitle = name + "_" + date;


            return View(model);
        }

        public async Task<IActionResult> InformPayment(string batchId, string sellerId, string value)
        {
            //Region info
            var style = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands;
            var provider = new CultureInfo("pt-BR");
            if (value.Contains("."))
            {
                provider = new CultureInfo("en-US");
            }

            //Get Amount
            decimal amount = decimal.Parse(value, style, provider);

            //Get Batch
            var batch = await _batchService.GetById(int.Parse(batchId));
            
            //Get total paid
            decimal paidValue = 0;
            foreach (var flow in batch.FlowList)
            {
                paidValue += flow.Amount;
            }

            if (amount <= batch.TotalValue - paidValue)
            {
                //Add Flow to FlowList of the Batch
                await _cashFlowService.AddFlow(batch.CashFlow, new Flow()
                {
                    Batch = batch,
                    Amount = amount,
                    CashFlow = batch.CashFlow,
                    Date = DateTime.Now,
                    EmployeeUser = await userManager.FindByIdAsync(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value),
                    Description = "Pagamento do Lote " + batchId
                });
            }
            else
            {
                //Viewbag Warning
            }

            return RedirectToAction("Details", "Seller", new { id = sellerId });
        }
    }
}