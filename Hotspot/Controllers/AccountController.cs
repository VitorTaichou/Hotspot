using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hotspot.Model;
using Hotspot.Model.Model;
using Hotspot.Models;
using Hotspot.Models.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Hotspot.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<EmployeeUser> userManager;
        private readonly SignInManager<EmployeeUser> signInManager;
        private readonly ILog _logService;

        public AccountController(UserManager<EmployeeUser> userManager, SignInManager<EmployeeUser> signInManager, ILog logService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this._logService = logService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsUsernameInUse(string username)
        {
            var user = await userManager.FindByNameAsync(username);
            
            if(user == null)
            {
                return Json(true);
            }
            else
            {
                return Json("Este usuário já está em uso");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new EmployeeUser
                {
                    UserName = model.Username
                };

                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, model.Role).Wait();
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            if (signInManager.IsSignedIn(User))
            {
                string userNameUpperCase = User.Identity.Name.First().ToString().ToUpper() + User.Identity.Name.Substring(1);
                await _logService.Add(new Log()
                {
                    User = userNameUpperCase,
                    Time = DateTime.Now,
                    Action = "Criou o Usuário " + model.Username + " do tipo " + model.Role
                });
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = new EmployeeUser
                {
                    UserName = model.Username
                };

                var result = await signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    if(!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    
                }

                ModelState.AddModelError(string.Empty, "Usuário ou Senha incorretos!");
            }

            return View(model);
        }

        public IActionResult Index()
        {
            AccountListViewModel model = new AccountListViewModel();
            List<AccountListItemViewModel> list = new List<AccountListItemViewModel>();
            var l = userManager.Users;

            foreach(var user in l)
            {
                list.Add(new AccountListItemViewModel()
                {
                    Id = user.Id,
                    Name = user.UserName
                });
            }
            model.AccountList = list;
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            RegisterViewModel model = new RegisterViewModel();

            var u = await userManager.FindByIdAsync(id);
            model.Username = u.UserName;
            model.Id = id;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RegisterViewModel model) 
        {
            var code = await userManager.GeneratePasswordResetTokenAsync(await userManager.FindByIdAsync(model.Id));

            var result = await userManager.ResetPasswordAsync(await userManager.FindByIdAsync(model.Id), code, model.Password);

            return RedirectToAction("Index", "Account");
        }

        public async Task<IActionResult> Delete(string id)
        {
            await userManager.DeleteAsync(await userManager.FindByIdAsync(id));

            return RedirectToAction("Index", "Account");
        }
    }
}