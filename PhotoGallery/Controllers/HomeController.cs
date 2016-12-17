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
            return View();
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
        [HttpPost]
        public ActionResult Search(string title_user)
        {
            ViewBag.Message = "Home";
            using (var database = new ApplicationDbContext())
            {
                var photo = database.Photos
                    .FirstOrDefault(p => p.Title == title_user);
                if(photo==null)
                {
                    photo = database.Photos.FirstOrDefault(p => p.Author.FullName == title_user);
                    if (photo != null)
                    {
                       return  RedirectToAction("ResultGallery","Photos", new { photo.AuthorId } );
                    }
                   
                }
                if (photo != null)
                {
                    return RedirectToAction("Details", "Photos", new { photo.Id });
                }
                return RedirectToAction("Index", "Home");
            }
        }

    }
}