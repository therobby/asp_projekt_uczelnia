using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using APS.Models;
using APS.Services;
using System.Text;
using System.IO;
using Microsoft.AspNetCore.Hosting;

// http://dotnetlearners.com/blogs/login-page-example-in-mvc-using-entity-frame-work

namespace APS.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserService userService;
        private readonly LostService lostService;
        private readonly ILogger<HomeController> _logger;
        private IWebHostEnvironment _hostingEnvironment;

        public HomeController(ILogger<HomeController> logger, UserService userService, LostService lostService, IWebHostEnvironment environment)
        {
            this.userService = userService;
            this.lostService = lostService;
            _hostingEnvironment = environment;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult List()
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
                    ViewBag.Admin = true;
                }
                else
                {

                    ViewBag.Admin = false;
                }
            }

            ViewBag.Losts = lostService.Get();

            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Lost(string id)
        {
            ViewBag.LostID = id;
            string uid = "";
            if (HttpContext.Session.Keys.Contains("UserID"))
            {
                byte[] idByte;
                HttpContext.Session.TryGetValue("UserID", out idByte);
                uid = Encoding.ASCII.GetString(idByte);
            } else
            {
                ViewBag.Editable = false;
            }

            var l = lostService.Get(id);
            if(uid != "" && (l.DodanaPrzez == uid || userService.Get(uid).Rola.ToLower().Trim() == "admin"))
            {
                ViewBag.Editable = true;
            } 

            var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
            try
            {
                var f = System.IO.File.ReadAllBytes(Path.Combine(uploads, l.Photo).Trim());
                var ext = Path.GetExtension(Path.Combine(uploads, l.Photo).Trim());
                string file = "data:image/" + ext + ";base64, "+Convert.ToBase64String(f);

                ViewData.Model = new LostDisplay
                {
                    Id = l.Id,
                    Imie = l.Imie,
                    Nazwisko = l.Nazwisko,
                    Opis = l.Opis,
                    OstatniaLokalizacja = l.OstatniaLokalizacja,
                    Photo = file,
                    Plec = l.Plec
                };
            }
            catch(Exception e)
            {

                ViewData.Model = new LostDisplay
                {
                    Id = l.Id,
                    Imie = l.Imie,
                    Nazwisko = l.Nazwisko,
                    Opis = l.Opis,
                    OstatniaLokalizacja = l.OstatniaLokalizacja,
                    Photo = "",
                    Plec = l.Plec
                };
            }

            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}


// https://docs.microsoft.com/pl-pl/aspnet/core/tutorials/first-mongo-app?view=aspnetcore-3.1&tabs=visual-studio

// https://docs.microsoft.com/pl-pl/aspnet/mvc/overview/security/create-an-aspnet-mvc-5-web-app-with-email-confirmation-and-password-reset