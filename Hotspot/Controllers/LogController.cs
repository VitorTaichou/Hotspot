using Hotspot.Model;
using Hotspot.Models.Log;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Hotspot.Controllers
{
    public class LogController : Controller
    {
        private readonly ILog _logService;

        public LogController(ILog logService)
        {
            _logService = logService;
        }

        public IActionResult Index()
        {
            var logList = _logService.GetAll();
            List<LogViewModel> list = new List<LogViewModel>();
            LogListViewModel model = new LogListViewModel();
            model.LogList = list;

            foreach(var log in logList)
            {
                list.Add(new LogViewModel()
                {
                    Id = log.Id,
                    User = log.User, 
                    Action = log.Action,
                    Time = log.Time
                });   
            }

            model.LogList = list.OrderByDescending(l => l.Time).ToList();
            return View(model);
        }
    }
}
