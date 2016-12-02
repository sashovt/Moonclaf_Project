using PhotoGallery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhotoGallery.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var db = new ApplicationDbContext();
            var photo = db.Photos.OrderByDescending(p => p.DateAdded).Take(3);
            ViewBag.Message = "Home";
            return View(photo.ToList());
        }
        public ActionResult About()
        {
            ViewBag.Message = "About";
            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Contact";
            return View();
        }

    }
}