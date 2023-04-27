using Hotspot.Model;
using Hotspot.Model.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Hotspot.Controllers
{
    public class SaleController : Controller
    {
        private readonly ISale _saleService;
        private readonly IBatch _batchService;
        private readonly ILog _logService;
        private readonly SignInManager<EmployeeUser> signInManager;

        public SaleController(ISale saleService, ILog logservice, SignInManager<EmployeeUser> signInManager, IBatch batchService)
        {
            _saleService = saleService;
            _logService = logservice;
            this.signInManager = signInManager;
            _batchService = batchService;
        }

        public async Task<IActionResult> PaymentBatch(int batch, int seller, string value)
        {
            //Get Batch
            var b = await _batchService.GetById(batch);
            
            await _saleService.InformPaymentByBatchId(batch, decimal.Parse(value));

            //Log Registration
            if (signInManager.IsSignedIn(User))
            {
                string userNameUpperCase = User.Identity.Name.First().ToString().ToUpper() + User.Identity.Name.Substring(1);
                await _logService.Add(new Log()
                {
                    User = userNameUpperCase,
                    Time = DateTime.Now,
                    Action = "Informou o pagamento do Lote " + batch
                });
            }

            return RedirectToAction("Details", "Seller", new { id = seller });
        }

        public async Task<IActionResult> PaymentReport(int id, decimal value)
        {
            await _saleService.InformPayment(id, value);
            var sale = await _saleService.GetById(id);

            //Log Registration
            if (signInManager.IsSignedIn(User))
            {
                string userNameUpperCase = User.Identity.Name.First().ToString().ToUpper() + User.Identity.Name.Substring(1);
                await _logService.Add(new Log()
                {
                    User = userNameUpperCase,
                    Time = DateTime.Now,
                    Action = "Informou o pagamento de " + sale.TotalValue.ToString("C2")
                });
            }

            return RedirectToAction("Index", "Report");
        }
    }
}