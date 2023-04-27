using Hotspot.Model;
using Hotspot.Models.Ticket;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hotspot.Controllers
{
    public class TicketController : Controller
    {
        private readonly ITicket _ticketService;
        private readonly IHistory _historyService;
        private readonly ICourtesy _courtesyService;
        private readonly ICatalogTicket _catalogTicketService;

        public TicketController(ITicket ticketService, IHistory historyService, ICourtesy courtesyService, ICatalogTicket catalogTicketService)
        {
            _ticketService = ticketService;
            _historyService = historyService;
            _courtesyService = courtesyService;
            _catalogTicketService = catalogTicketService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(TicketIndexViewModel model)
        {
            if(model.Search != null)
            {
                if (model.Search.Length == 8)
                {
                    //Ticket
                    ViewBag.IsTicket = true;
                    var ticket = await _ticketService.GetByPassword(model.Search);
                    List<TicketConnectionHistoryViewModel> connList = new List<TicketConnectionHistoryViewModel>();
                    List<TicketLogoutHistoryViewModel> logoutList = new List<TicketLogoutHistoryViewModel>();

                    if (ticket != null)
                    {
                        var connHistory = _historyService.GetConnectionHistoryByTicket(model.Search);
                        var logoutHistory = _historyService.GetLogoutHistoryByTicket(model.Search);

                        if (connHistory != null)
                        {
                            foreach (var connection in connHistory)
                            {
                                connList.Add(new TicketConnectionHistoryViewModel()
                                {
                                    ConnectionTime = connection.ConnectionTime,
                                    TimeLeft = connection.TimeLeft
                                });
                            }
                        }

                        if (logoutHistory != null)
                        {
                            foreach (var logout in logoutHistory)
                            {
                                logoutList.Add(new TicketLogoutHistoryViewModel()
                                {
                                    LogoutTime = logout.LogoutTime,
                                    TimeUsed = logout.TimeUsed
                                });
                            }
                        }
                        decimal f = ticket.Franchise / 1048576;
                        model = new TicketIndexViewModel()
                        {
                            Search = model.Search,
                            Ticket = new TicketViewModel()
                            {
                                BatchId = ticket.Batch.Id,
                                Code = ticket.Code,
                                SellerName = ticket.Batch.Seller.Name + " " + ticket.Batch.Seller.Surname,
                                TimeLeft = ticket.Time,
                                ConnectionHistory = connList,
                                LogoutHistory = logoutList,
                                Franchise = Math.Round(f, 1).ToString()
                            }
                        };
                    }
                    else
                    {
                        model.Ticket = null;
                    }
                }
                else if (model.Search.Length == 6)
                {
                    //Courtesy
                    ViewBag.IsTicket = false;
                    var ticket = await _courtesyService.GetByPassword(model.Search);
                    List<TicketConnectionHistoryViewModel> connList = new List<TicketConnectionHistoryViewModel>();
                    List<TicketLogoutHistoryViewModel> logoutList = new List<TicketLogoutHistoryViewModel>();

                    if (ticket != null)
                    {
                        var connHistory = _historyService.GetConnectionHistoryByTicket(model.Search);
                        var logoutHistory = _historyService.GetLogoutHistoryByTicket(model.Search);

                        if (connHistory != null)
                        {
                            foreach (var connection in connHistory)
                            {
                                connList.Add(new TicketConnectionHistoryViewModel()
                                {
                                    ConnectionTime = connection.ConnectionTime,
                                    TimeLeft = connection.TimeLeft
                                });
                            }
                        }

                        if (logoutHistory != null)
                        {
                            foreach (var logout in logoutHistory)
                            {
                                logoutList.Add(new TicketLogoutHistoryViewModel()
                                {
                                    LogoutTime = logout.LogoutTime,
                                    TimeUsed = logout.TimeUsed
                                });
                            }
                        }

                        model = new TicketIndexViewModel()
                        {
                            Search = model.Search,
                            Ticket = new TicketViewModel()
                            {
                                BatchId = 0,
                                Code = ticket.Code,
                                SellerName = ticket.Name + " " + ticket.Surname,
                                TimeLeft = TimeSpan.FromDays(1),
                                ConnectionHistory = connList,
                                LogoutHistory = logoutList
                            }
                        };
                    }
                    else
                    {
                        model.Ticket = null;
                    }
                }
                else if(model.Search.Length == 9)
                {
                    //Catalog Ticket
                    ViewBag.IsTicket = true;
                    var ticket = _catalogTicketService.GetByPassword(model.Search);
                    List<TicketConnectionHistoryViewModel> connList = new List<TicketConnectionHistoryViewModel>();
                    List<TicketLogoutHistoryViewModel> logoutList = new List<TicketLogoutHistoryViewModel>();

                    if (ticket != null)
                    {
                        var connHistory = _historyService.GetConnectionHistoryByTicket(model.Search);
                        var logoutHistory = _historyService.GetLogoutHistoryByTicket(model.Search);

                        if (connHistory != null)
                        {
                            foreach (var connection in connHistory)
                            {
                                connList.Add(new TicketConnectionHistoryViewModel()
                                {
                                    ConnectionTime = connection.ConnectionTime,
                                    TimeLeft = connection.TimeLeft
                                });
                            }
                        }

                        if (logoutHistory != null)
                        {
                            foreach (var logout in logoutHistory)
                            {
                                logoutList.Add(new TicketLogoutHistoryViewModel()
                                {
                                    LogoutTime = logout.LogoutTime,
                                    TimeUsed = logout.TimeUsed
                                });
                            }
                        }

                        decimal f = ticket.Franchise / 1048576;
                        model = new TicketIndexViewModel()
                        {
                            Search = model.Search,
                            Ticket = new TicketViewModel()
                            {
                                BatchId = ticket.Batch.Id,
                                Code = ticket.Code,
                                SellerName = ticket.Batch.Seller.Name + " " + ticket.Batch.Seller.Surname,
                                TimeLeft = TimeSpan.FromSeconds(ticket.Time),
                                ConnectionHistory = connList,
                                LogoutHistory = logoutList,
                                Franchise = Math.Round(f, 1).ToString()
                            }
                        };
                    }
                    else
                    {
                        model.Ticket = null;
                    }
                }
            }

            return View(model);
        }
    }
}