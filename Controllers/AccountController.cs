using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UsersApp.ViewModels;
using UsersApp.Models;
using Microsoft.AspNetCore.Identity;

namespace UsersApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User { EmailConfirmed = true, Email = model.Email, UserName = model.UserName, ReqistrationTime = DateTime.Now, LastLoginTime = null, Status = "active" };
                // добавляем пользователя
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // установка куки
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                User u = await _userManager.FindByEmailAsync(model.Email);
                if (u != null)
                {
                    if (u.Status.Equals("blocked"))
                    {
                        return View(model);
                    }
                    var result =
                        await _signInManager.PasswordSignInAsync(u.UserName, model.Password, true, false);
                    if (result.Succeeded)
                    {
                        User user = await _userManager.FindByEmailAsync(model.Email);
                        if (user != null)
                        {
                            user.LastLoginTime = DateTime.Now;
                            IdentityResult res = await _userManager.UpdateAsync(user);
                        }

                        if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                        {
                            return Redirect(model.ReturnUrl);
                        }
                        else
                        {
                            ViewData["Welcome"] = "Hi, " + model.Email;
                            return RedirectToAction("Index", "Users");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid email or password.");
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // удаляем аутентификационные куки
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
