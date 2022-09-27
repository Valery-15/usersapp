using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using UsersApp.Models;
using UsersApp.ViewModels;

namespace UsersApp.Controllers
{
    public class UsersController : Controller
    {
        UserManager<User> _userManager;

        public UsersController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }


        public IActionResult Index() {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            var name = _userManager.GetUserName(currentUser);
            ViewData["Welcome"] = "Hi, " + name;
            return View(_userManager.Users.ToList());
        }

        


        [HttpGet]
        public async Task<ActionResult> Lock(string[] id)
        {
            var ID = _userManager.GetUserId(this.User);
            User currentUser = await _userManager.FindByIdAsync(ID);
            if (currentUser != null)
            {
                if (currentUser.Status.Equals("active"))
                {
                    bool lockYourself = false;
                    foreach (string Id in id)
                    {
                        if (Id.Equals(ID))
                        {
                            lockYourself = true;
                        }
                        User user = await _userManager.FindByIdAsync(Id);
                        if (user != null)
                        {
                            user.Status = "blocked";
                            IdentityResult result = await _userManager.UpdateAsync(user);
                        }
                    }
                    if (lockYourself)
                    {
                        return RedirectToAction("Logout", "Account");
                    }
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public async Task<ActionResult> Unlock(string[] id)
        {
            var ID = _userManager.GetUserId(this.User);
            User currentUser = await _userManager.FindByIdAsync(ID);
            if (currentUser != null)
            {
                if (currentUser.Status.Equals("active"))
                {
                    foreach (string Id in id)
                    {
                        User user = await _userManager.FindByIdAsync(Id);
                        if (user != null)
                        {
                            user.Status = "active";
                            IdentityResult result = await _userManager.UpdateAsync(user);
                        }
                    }
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public async Task<ActionResult> Delete(string[] id)
        {
            var ID = _userManager.GetUserId(this.User);
            User currentUser = await _userManager.FindByIdAsync(ID);
            if (currentUser != null)
            {
                if (currentUser.Status.Equals("active"))
                {
                    
                    bool deleteYourself = false;
                    foreach (string Id in id)
                    {
                        if (Id.Equals(ID))
                        {
                            deleteYourself = true;
                           
                        }
                        else
                        {
                            User user = await _userManager.FindByIdAsync(Id);
                            if (user != null)
                            {
                                IdentityResult result = await _userManager.DeleteAsync(user);
                            }
                        }
                    }
                    if (deleteYourself)
                    {
                        User user = await _userManager.FindByIdAsync(ID);
                        if (user != null)
                        {
                            IdentityResult result = await _userManager.DeleteAsync(user);
                        }
                        return RedirectToAction("Login", "Account");
                    } else
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            return RedirectToAction("Login", "Account");
        }
    }
}
