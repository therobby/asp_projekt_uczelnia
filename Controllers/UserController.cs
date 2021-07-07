using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using APS.Models;
using APS.Services;
using System.Text;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace APS.Controllers
{
    public class UserController : Controller
    {
        private readonly UserService userService;
        private readonly LostService lostService;
        private IWebHostEnvironment _hostingEnvironment;

        public UserController(UserService userService, LostService lostService, IWebHostEnvironment environment)
        {
            this.userService = userService;
            this.lostService = lostService;
            _hostingEnvironment = environment;

        }

        public IActionResult AddLost()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddL(LostInput lostModel)
        {
            byte[] idByte;
            HttpContext.Session.TryGetValue("UserID", out idByte);
            string id = Encoding.ASCII.GetString(idByte);
            lostModel.DodanaPrzez = id;

            Lost lost = new Lost
            {
                Id = lostModel.Id,
                Imie = lostModel.Imie,
                Nazwisko = lostModel.Nazwisko,
                Opis = lostModel.Opis,
                OstatniaLokalizacja = lostModel.OstatniaLokalizacja,
                Plec = lostModel.Plec,
                DodanaPrzez = lostModel.DodanaPrzez
            };
            var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
            var filePath = Path.Combine(uploads, lostModel.DodanaPrzez + lostModel.Photo.FileName + lostModel.Photo.ContentType.Replace("image/", "."));
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await lostModel.Photo.CopyToAsync(fileStream);
            }
            lost.Photo = lostModel.DodanaPrzez + lostModel.Photo.FileName + lostModel.Photo.ContentType.Replace("image/", ".");

            if(lostService.Get(lost.Id) != null)
            {
                lostService.Update(lost.Id, lost);
            }
            else
            {
                lostService.Create(lost);
            }
            return RedirectToAction("List", "Home");
        }


        public IActionResult DeleteLost(string id)
        {
            string uid = "";
            if (HttpContext.Session.Keys.Contains("UserID"))
            {
                byte[] idByte;
                HttpContext.Session.TryGetValue("UserID", out idByte);
                uid = Encoding.ASCII.GetString(idByte);
            }
            else
            {
                return RedirectToAction("List", "Home");
            }

            var l = lostService.Get(id);
            if (uid != "" && (l.DodanaPrzez == uid || userService.Get(uid).Rola.ToLower().Trim() == "admin"))
            {
                lostService.Remove(lostService.Get(id));
            }
            return RedirectToAction("List", "Home");
        }

        public IActionResult EditLost(string id)
        {
            var l = lostService.Get(id);

            var model = new LostInput
            {
                Id = l.Id,
                Imie = l.Imie,
                Nazwisko = l.Nazwisko,
                DodanaPrzez = l.DodanaPrzez,
                Opis = l.Opis,
                OstatniaLokalizacja = l.OstatniaLokalizacja,
                Plec = l.Plec
            };

            return View("AddLost", model);
        }
    }
}