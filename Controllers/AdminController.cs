using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using APS.Models;
using APS.Services;
using System.Text;

namespace APS.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserService userService;
        private readonly LostService lostService;

        public AdminController(UserService userService, LostService lostService)
        {
            this.userService = userService;
            this.lostService = lostService;

        }
        public IActionResult UserList()
        {
            bool isLoggedIn = HttpContext.Session.Keys.Contains("UserID");
            ViewBag.LoggedIn = isLoggedIn;
            if (isLoggedIn)
            {
                byte[] idByte;
                HttpContext.Session.TryGetValue("UserID", out idByte);
                string id = Encoding.ASCII.GetString(idByte);
                if (userService.Get(id).Rola.ToLower().Trim() == "admin")
                {
                    ViewBag.Users = userService.Get();
                    return View();
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        public IActionResult UserEdit(string id)
        {
            bool isLoggedIn = HttpContext.Session.Keys.Contains("UserID");
            if (isLoggedIn)
            {
                byte[] idByte;
                HttpContext.Session.TryGetValue("UserID", out idByte);
                string uid = Encoding.ASCII.GetString(idByte);
                if (userService.Get(uid).Rola.ToLower().Trim() == "admin")
                {
                    var usr = userService.Get(id);
                    var model = new EditUser
                    {
                        Id = usr.Id,
                        Imie = usr.Imie,
                        Nazwisko = usr.Nazwisko,
                        Email = usr.Email,
                        NumerTelefonu = usr.NumerTelefonu,
                        Plec = usr.Plec,
                        Rola = usr.Rola
                    };
                    return View(model);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult SaveUserEdit(EditUser user)
        {

            bool isLoggedIn = HttpContext.Session.Keys.Contains("UserID");
            if (isLoggedIn)
            {
                byte[] idByte;
                HttpContext.Session.TryGetValue("UserID", out idByte);
                string uid = Encoding.ASCII.GetString(idByte);
                if (userService.Get(uid).Rola.ToLower().Trim() == "admin")
                {
                    var nusr = userService.Get(user.Id);
                    nusr.Imie = nusr.Imie;
                    nusr.Nazwisko = nusr.Nazwisko;
                    nusr.NumerTelefonu = nusr.NumerTelefonu;
                    nusr.Plec = nusr.Plec;
                    nusr.Rola = nusr.Rola;
                    userService.Update(user.Id, nusr);

                    return RedirectToAction("UserList");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult DeleteUser(string id)
        {

            bool isLoggedIn = HttpContext.Session.Keys.Contains("UserID");
            if (isLoggedIn)
            {
                byte[] idByte;
                HttpContext.Session.TryGetValue("UserID", out idByte);
                string uid = Encoding.ASCII.GetString(idByte);
                if (userService.Get(uid).Rola.ToLower().Trim() == "admin")
                {
                    userService.Remove(userService.Get(id));
                    return RedirectToAction("UserList");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}