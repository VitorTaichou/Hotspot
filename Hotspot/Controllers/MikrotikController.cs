using Hotspot.Model;
using Hotspot.Model.Model;
using Hotspot.Models.Mikrotik;
using Hotspot.Tools.Mikrotik;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Hotspot.Controllers
{
    public class MikrotikController : Controller
    {
        private readonly ITicket _ticketService;
        private readonly IHistory _historyService;
        private readonly ICourtesy _courtesyService;
        private readonly ISpecialTicket _specialTicketService;
        private readonly ICatalogTicket _catalogTicketService;
        private readonly ITicketsConfiguration _ticketsConfiguration;

        public MikrotikController(ITicket ticketService, IHistory historyService, 
            ICourtesy courtesyService, ISpecialTicket specialTicketService, ICatalogTicket catalogTicketService, 
            ITicketsConfiguration ticketsConfiguration)
        {
            _ticketService = ticketService;
            _historyService = historyService;
            _courtesyService = courtesyService;
            _specialTicketService = specialTicketService;
            _catalogTicketService = catalogTicketService;
            _ticketsConfiguration = ticketsConfiguration;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Fail(string password)
        {
            if(password != null)
            {
                MikrotikLoginViewModel model = new MikrotikLoginViewModel(null, password, TimeSpan.FromSeconds(0));
                return View(model);
            }
            else
            {
                return View(null);
            }
        }

        //Test Link
        //https://localhost:44329/mikrotik/login?ip=192.168.150.1&password=191E1D2HK
        //mikrotik/login?ip=192.168.1.2&password=999999
        [AllowAnonymous]
        public async Task<IActionResult> Login(string ip, string password)
        {
            MikrotikHandler mikrotikHandler = new MikrotikHandler();
            password = password.ToUpper();

            if (password.Length == 8) //Default Tickets
            {
                //Check if password exists
                var ticket = await _ticketService.GetByPassword(password); 
                if (ticket != null)
                {
                    if(ticket.Time.TotalSeconds > 0 && ticket.Franchise > 0)
                    {
                        await mikrotikHandler.CreateHotspotUser(password, ip, ticket.Time, ticket.Franchise, "default");
                        MikrotikLoginViewModel model = new MikrotikLoginViewModel(ip, password, ticket.Time);

                        //Register Login
                        await _historyService.RegisterConnection(new ConnectionHistory()
                        {
                            Code = password,
                            ConnectionTime = DateTime.Now,
                            TimeLeft = ticket.Time
                        });
                        model.Franchise = ticket.Franchise.ToString();
                        return View(model);
                    }
                    else
                    {
                        return RedirectToAction("Fail", "Mikrotik", new { password = password });
                    }
                }
            }
            else if(password.Length == 6) //Courtesy
            {
                //Check if Courtesy exists
                var ticket = await _courtesyService.GetByPassword(password);
                if (ticket != null)
                {
                    try
                    {
                        await mikrotikHandler.CreateHotspotUser(password, ip, TimeSpan.FromHours(23), long.Parse(_ticketsConfiguration.Get().DefaultFranchise) * 1048576, "default");
                    }
                    catch(Exception e)
                    {

                    }

                    MikrotikLoginViewModel model = new MikrotikLoginViewModel(ip, password, TimeSpan.FromHours(23));

                    //Register Login
                    await _historyService.RegisterConnection(new ConnectionHistory()
                    {
                        Code = password,
                        ConnectionTime = DateTime.Now,
                        TimeLeft = TimeSpan.FromHours(23)
                    });

                    return View(model);
                }
            }
            else if(password.Length == 7) //Special Ticket
            {
                //Check if password exists
                var ticket = await _specialTicketService.GetByPassword(password);
                if (ticket != null)
                {
                    if(ticket.FirstUse)
                    {
                        if(DateTime.Now < ticket.DueDate)
                        {
                            await mikrotikHandler.CreateHotspotUser(password, ip, TimeSpan.FromHours(23), long.Parse(_ticketsConfiguration.Get().DefaultFranchise) * 1048576, "default");
                            MikrotikLoginViewModel model = new MikrotikLoginViewModel(ip, password, TimeSpan.FromHours(23));

                            //Register Login
                            await _historyService.RegisterConnection(new ConnectionHistory()
                            {
                                Code = password,
                                ConnectionTime = DateTime.Now,
                                TimeLeft = TimeSpan.FromHours(23)
                            });

                            return View(model);
                        }
                        else
                        {
                            return RedirectToAction("Fail", "Mikrotik", new { password = password });
                        }
                    }
                    else
                    {
                        await _specialTicketService.SetFirstUse(ticket);
                        await mikrotikHandler.CreateHotspotUser(password, ip, TimeSpan.FromHours(23), long.Parse(_ticketsConfiguration.Get().DefaultFranchise) * 1048576, "default");
                        MikrotikLoginViewModel model = new MikrotikLoginViewModel(ip, password, TimeSpan.FromHours(23));

                        //Register Login
                        await _historyService.RegisterConnection(new ConnectionHistory()
                        {
                            Code = password,
                            ConnectionTime = DateTime.Now,
                            TimeLeft = TimeSpan.FromHours(23)
                        });

                        return View(model);
                    }
                }
            }
            else if (password.Length == 9) //Catalog Tickets
            {
                //Check if password exists
                var ticket = _catalogTicketService.GetByPassword(password);
                if (ticket != null)
                {
                    if (ticket.FirstUse)
                    {
                        if (ticket.Time > 0 && ticket.Franchise > 0 && DateTime.Now < ticket.DueDate)
                        {
                            await mikrotikHandler.CreateHotspotUser(password, ip, TimeSpan.FromSeconds(ticket.Time), ticket.Franchise, ticket.ProfileName);
                            MikrotikLoginViewModel model = new MikrotikLoginViewModel(ip, password, TimeSpan.FromSeconds(ticket.Time));

                            //Register Login
                            await _historyService.RegisterConnection(new ConnectionHistory()
                            {
                                Code = password,
                                ConnectionTime = DateTime.Now,
                                TimeLeft = TimeSpan.FromHours(23)
                            });

                            model.Franchise = ticket.Franchise.ToString();
                            return View(model);
                        }
                        else
                        {
                            return RedirectToAction("Fail", "Mikrotik", new { password = password });
                        }
                    }
                    else
                    {
                        await _catalogTicketService.SetFirstUse(ticket);
                        await mikrotikHandler.CreateHotspotUser(password, ip, TimeSpan.FromSeconds(ticket.Time), ticket.Franchise, ticket.ProfileName);
                        MikrotikLoginViewModel model = new MikrotikLoginViewModel(ip, password, TimeSpan.FromHours(23));

                        //Register Login
                        await _historyService.RegisterConnection(new ConnectionHistory()
                        {
                            Code = password,
                            ConnectionTime = DateTime.Now,
                            TimeLeft = TimeSpan.FromHours(23)
                        });

                        return View(model);
                    }
                }
            }

            return RedirectToAction("Fail", "Mikrotik", new { });
        }

        [AllowAnonymous]
        public async Task<IActionResult> Logout(string password, int time, string franchise)
        {
            if(password.Length == 9)
            {
                await _catalogTicketService.UpdateTicketData(password, time, franchise);
            }
            else
            {
                await _ticketService.UpdateTimeByPassword(password, time);
            }

            //Register Logout
            await _historyService.RegisterLogout(new LogoutHistory()
            {
                Code = password,
                TimeUsed = TimeSpan.FromSeconds(time),
                LogoutTime = DateTime.Now
            });

            return View();
        }

        [AllowAnonymous]
        public async Task<IActionResult> Sync(string ip)
        {
            MikrotikHandler mikrotikHandler = new MikrotikHandler();
            var list = await mikrotikHandler.UpdateTimeAndFranchise(_ticketsConfiguration.Get().DefaultIp);
            
            foreach(var item in list)
            {
                //6d23h51m30s
                //Convert MK timestamp to seconds
                string time = item.SessionTimeLeft;
                int seconds = 0;

                if (time.IndexOf("d") != -1)
                {
                    seconds += 86400 * Int32.Parse(time.Split("d")[0]);
                    time = time.Split("d")[1];
                }

                if (time.IndexOf("h") != -1)
                {
                    seconds += 3600 * Int32.Parse(time.Split("h")[0]);
                    time = time.Split("h")[1];
                }

                if (time.IndexOf("m") != -1)
                {
                    seconds += 60 * Int32.Parse(time.Split("m")[0]);
                    time = time.Split("m")[1];
                }

                if (time.IndexOf("s") != -1)
                {
                    seconds += Int32.Parse(time.Split("s")[0]);
                }

                if (item.UserName.Length == 9)
                {
                    await _catalogTicketService.SetTicketData(item.UserName, seconds, (item.BytesIn + item.BytesOut).ToString());
                }
                else
                {
                    await _ticketService.SetTimeByPassword(item.UserName, seconds);
                }
            }

            return View();
        }
    }
}