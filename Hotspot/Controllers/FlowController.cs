using Hotspot.Model;
using Hotspot.Model.Model;
using Hotspot.Models.Flow;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hotspot.Controllers
{
    public class FlowController : Controller
    {
        private readonly ICashFlow _cashFlowservice;

        public FlowController(ICashFlow cashFlowservice)
        {
            _cashFlowservice = cashFlowservice;
        }

        [HttpGet]
        public IActionResult Index()
        {
            FlowListViewModel model = new FlowListViewModel();

            var flowList = _cashFlowservice.GetAllFlow();
            decimal totalAmount = 0;

            List<FlowViewModel> modelList = new List<FlowViewModel>();

            //Seed flowList
            foreach (var flow in flowList)
            {
                FlowViewModel f = new FlowViewModel()
                {
                    Amount = flow.Amount.ToString("C2"),
                    CashFlow = new Models.CashFlow.CashFlowViewModel()
                    {
                        Name = flow.CashFlow.Name,
                    },
                    Description = flow.Description,
                    Id = flow.Id,
                    Date = flow.Date
                };

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
                modelList.Add(f);
            }
            model.FlowViewModelList = modelList.OrderByDescending(l => l.Date).ToList();
            model.TotalAmount = totalAmount;
            model.StartDate = model.EndDate = DateTime.Now;
            model.CurrentAmount = model.TotalInflow + model.TotalOutflow;

            return View(model);
        }

        [HttpPost]
        public IActionResult Index(FlowListViewModel model)
        {
            var flowList = _cashFlowservice.GetAllFlow();

            DateTime start = model.StartDate;
            DateTime end = model.EndDate + new TimeSpan(23, 59, 59);

            flowList = flowList.Where(f => f.Date <= end).Where(f => f.Date >= start);
            decimal totalAmount = 0;

            List<FlowViewModel> modelList = new List<FlowViewModel>();

            //Seed flowList
            foreach (var flow in flowList)
            {
                FlowViewModel f = new FlowViewModel()
                {
                    Amount = flow.Amount.ToString("C2"),
                    CashFlow = new Models.CashFlow.CashFlowViewModel()
                    {
                        Name = flow.CashFlow.Name,
                    },
                    Description = flow.Description,
                    Id = flow.Id,
                    Date = flow.Date
                };

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
                modelList.Add(f);
            }
            model.FlowViewModelList = modelList.OrderByDescending(l => l.Date).ToList();
            model.TotalAmount = totalAmount;
            model.StartDate = model.EndDate = DateTime.Now;
            model.CurrentAmount = model.TotalInflow + model.TotalOutflow;

            return View(model);
        }
    }
}
