using Hotspot.Model;
using Hotspot.Models.Report;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hotspot.Controllers
{
    public class ReportController : Controller
    {
        private readonly ISale _saleService;
        private readonly ISeller _sellerService;
        private readonly IBatch _BatchService;

        public ReportController(ISale saleService, ISeller sellerService, IBatch batchService)
        {
            _saleService = saleService;
            _sellerService = sellerService;
            _BatchService = batchService;
        }

        public async Task<IActionResult> Payment(int id, decimal value)
        {
            await _saleService.InformPayment(id, value);

            return RedirectToAction("Index");
        }

        public IActionResult Index()
        {
            ReportListViewModel model = new ReportListViewModel();
            List<ReportObjectViewModel> dayPendencies = new List<ReportObjectViewModel>();
            List<ReportObjectViewModel> dayPaid = new List<ReportObjectViewModel>();
            List<ReportObjectViewModel> monthPendencies = new List<ReportObjectViewModel>();
            List<ReportObjectViewModel> monthPaid = new List<ReportObjectViewModel>();
            List<ReportObjectViewModel> lastMonthPendencies = new List<ReportObjectViewModel>();
            List<ReportObjectViewModel> lastMonthPaid = new List<ReportObjectViewModel>();

            model.DayPaid = dayPaid;
            model.DayPendencies = dayPendencies;
            model.MonthPaid = monthPaid;
            model.MonthPendencies = monthPendencies;
            model.LastMonthPaid = lastMonthPaid;
            model.LastMonthPendencies = lastMonthPendencies;

            var salesDay = _saleService.GetAllByDay(DateTime.Now);
            var salesMonth = _saleService.GetAllByMonth(DateTime.Now.Month, DateTime.Now.Year);

            int lastMonth = DateTime.Now.Month - 1;
            int year = DateTime.Now.Year;

            if(lastMonth == 0)
            {
                lastMonth = 12;
                year--;
            }

            var salesLastMonth = _saleService.GetAllByMonth(lastMonth, year);

            //Sales of the Day
            foreach (var sale in salesDay)
            {
                double totalValue = (sale.QtyOneHour * 2.0) + (sale.QtyTwoHour * 3.0) + (sale.QtyThreeHour * 5.0) + (sale.QtySixHour * 10.0);
                ReportObjectViewModel reportViewObject = new ReportObjectViewModel()
                {
                    BatchId = _BatchService.GetBySale(sale.Id).Id,
                    SaleDate = sale.Date,
                    SellerCity = sale.Seller.Address.Locale.City,
                    SellerFullName = sale.Seller.Name + " " + sale.Seller.Surname,
                    PaymentMethod = sale.PaymentMethod,
                    SaleId = sale.Id, 
                    TotalPayment = totalValue
                };

                if (sale.Payment)
                {
                    dayPaid.Add(reportViewObject);
                }
                else
                {
                    dayPendencies.Add(reportViewObject);
                }
            }

            //Sales of the Month
            foreach (var sale in salesMonth)
            {
                double totalValue = (sale.QtyOneHour * 2.0) + (sale.QtyTwoHour * 3.0) + (sale.QtyThreeHour * 5.0) + (sale.QtySixHour * 10.0);
                ReportObjectViewModel reportViewObject = new ReportObjectViewModel()
                {
                    BatchId = _BatchService.GetBySale(sale.Id).Id,
                    SaleDate = sale.Date,
                    SellerCity = sale.Seller.Address.Locale.City,
                    SellerFullName = sale.Seller.Name + " " + sale.Seller.Surname,
                    PaymentMethod = sale.PaymentMethod,
                    SaleId = sale.Id,
                    TotalPayment = totalValue
                };

                if (sale.Payment)
                {
                    monthPaid.Add(reportViewObject);
                }
                else
                {
                    monthPendencies.Add(reportViewObject);
                }
            }

            //Sales of the Last Month
            foreach (var sale in salesLastMonth)
            {
                double totalValue = (sale.QtyOneHour * 2.0) + (sale.QtyTwoHour * 3.0) + (sale.QtyThreeHour * 5.0) + (sale.QtySixHour * 10.0);
                ReportObjectViewModel reportViewObject = new ReportObjectViewModel()
                {
                    BatchId = _BatchService.GetBySale(sale.Id).Id,
                    SaleDate = sale.Date,
                    SellerCity = sale.Seller.Address.Locale.City,
                    SellerFullName = sale.Seller.Name + " " + sale.Seller.Surname,
                    PaymentMethod = sale.PaymentMethod,
                    SaleId = sale.Id,
                    TotalPayment = totalValue
                };

                if (sale.Payment)
                {
                    lastMonthPaid.Add(reportViewObject);
                }
                else
                {
                    lastMonthPendencies.Add(reportViewObject);
                }
            }

            return View(model);
        }
    }
}