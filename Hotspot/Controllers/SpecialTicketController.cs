using Hotspot.Model;
using Hotspot.Model.Model;
using Hotspot.Models.SpecialTicket;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hotspot.Controllers
{
    public class SpecialTicketController : Controller
    {
        private readonly ISpecialTicket _specialTicketService;
        private readonly ISeller _sellerService;
        private readonly ICashFlow _cashFlowService;
        private readonly ILog _logService;
        private readonly UserManager<EmployeeUser> userManager;
        private readonly SignInManager<EmployeeUser> signInManager;

        public SpecialTicketController(ISpecialTicket specialTicketService, 
            ISeller sellerService, ICashFlow cashFlowService, ILog logService, 
            UserManager<EmployeeUser> userManager, SignInManager<EmployeeUser> signInManager)
        {
            _specialTicketService = specialTicketService;
            _sellerService = sellerService;
            _cashFlowService = cashFlowService;
            _logService = logService;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public async Task<IActionResult> Add(int sellerId, int cashflowId)
        {
            //Data gather
            var seller = await _sellerService.GetById(sellerId);
            var cashFlow = await _cashFlowService.GetById(cashflowId);

            //Creating Flow
            var flow = new Flow()
            {
                Batch = null,
                Amount = 10,
                CashFlow = cashFlow,
                Date = DateTime.Now,
                EmployeeUser = await userManager.FindByIdAsync(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value),
                Description = "Ticket Especial criado para o vendedor " + seller.Name + " " + seller.Surname
            };
            await _cashFlowService.AddFlow(cashFlow, flow);

            //Creating the ticket
            SpecialTicket sTicket = new SpecialTicket()
            {
                FirstUse = false,
                Seller = seller,
                Flow = flow
            };
            await _specialTicketService.Add(sTicket);
            //TODO Log Registry

            return RedirectToAction("Details", "SpecialTicket", new { id = sellerId });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var sTicket = await _specialTicketService.GetById(id);
            int sellerId = sTicket.Seller.Id;

            await _cashFlowService.DeleteFlow(sTicket.Flow.Id);
            await _specialTicketService.Delete(id);
            
            return RedirectToAction("Details", "SpecialTicket", new { id = sellerId });
        }

        public async Task<IActionResult> Print(int id)
        {
            var ticket = await _specialTicketService.GetById(id);
            SpecialTicketViewModel model = new SpecialTicketViewModel()
            {
                Code = ticket.Code,
                DueDate = ticket.DueDate,
                FirstUse = ticket.FirstUse,
                Id = ticket.Id,
                SellerCity = ticket.Seller.Address.Locale.City,
                SellerId = ticket.Seller.Id,
                SellerName = ticket.Seller.Name + " " + ticket.Seller.Surname
            };

            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            //Returns a model containing a list of special tickets of this seller

            //Gather Data
            var seller = await _sellerService.GetById(id);
            var sTicketList = _specialTicketService.GetBySellerId(id);

            var cashFlowList = _cashFlowService.GetAll();
            var cf = cashFlowList.Where(c => c.Locale == seller.Address.Locale).First();

            //Create special Ticket List
            List<SpecialTicketViewModel> modelList = new List<SpecialTicketViewModel>();
            foreach(var sTicket in sTicketList)
            {
                modelList.Add(new SpecialTicketViewModel()
                {
                    Code = sTicket.Code,
                    DueDate = sTicket.DueDate,
                    FirstUse = sTicket.FirstUse,
                    Id = sTicket.Id,
                    SellerId = sTicket.Seller.Id
                });
            }

            SpecialTicketListViewModel model = new SpecialTicketListViewModel()
            {
                Id = seller.Id,
                Name = seller.Name,
                Surname = seller.Surname,
                SpecialTickets = modelList,
                CashFlowId = cf.Id
            };
            
            return View(model);
        }
    }
}
