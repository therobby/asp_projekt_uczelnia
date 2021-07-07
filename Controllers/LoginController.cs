using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APS.Models;
using APS.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
//using Microsoft.AspNetCore.Session;

namespace APS.Controllers
{
    public class LoginController : Controller
    {
        private readonly UserService userService;
        public LoginController(UserService userService)
        {
            this.userService = userService;
        }

        public IActionResult Index()
        {
            var mess = TempData["LoginMessage"];
            ViewData["LoginMessage"] = mess != null? mess.ToString() : "";
            return View();
        }

        [HttpPost]
        public IActionResult Authorize(User userModel)
        {
            if (!userService.LoginExists(userModel.Email))
            {
                TempData["LoginMessage"] = "User Doesn't Exists!";
                return RedirectToAction("Index", "Login");
            } else
            {
                var userID = userService.Login(userModel.Email, userModel.Password);
                if (userID != null)
                {
                    if(userService.Get(userID).Rola.ToLower().Trim() == "admin")
                    {
                        HttpContext.Session.Set("Admin", Encoding.ASCII.GetBytes("true"));
                    }
                    HttpContext.Session.Set("UserID", Encoding.ASCII.GetBytes(userID));
                    return RedirectToAction("Index", "Home");
                } else
                {
                    TempData["LoginMessage"] = "Invalid Credentials";
                    return RedirectToAction("Index", "Login");
                }
            }
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegisterUser(User userModel)
        {
            if (!userService.LoginExists(userModel.Email))
            {
                userModel.Rola = "User";
                userService.Create(userModel);
                TempData["LoginMessage"] = "Register Successful";
                return RedirectToAction("Index", "Login");
            } else
            {
                TempData["RegisterMessage"] = "User With This Email Already Exists!";
                return RedirectToAction("Register", "Login");
            }
        }

        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}