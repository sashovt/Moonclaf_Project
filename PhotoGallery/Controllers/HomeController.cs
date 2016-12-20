using PhotoGallery.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace PhotoGallery.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            back.backIndex = 5;
            var db = new ApplicationDbContext();
            var photo = db.Photos.OrderByDescending(p => p.DateAdded).Take(3);
            var categories = db.Categories.OrderBy(c => c.Name);


            return View(categories);
        }
        public ActionResult About()
        {
            return View();
        }
        public ActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Search(string title_user)
        {
            using (var database = new ApplicationDbContext())
            {
                var photo = database.Photos
                    .FirstOrDefault(p => p.Title == title_user);
                if (photo == null)
                {
                    try
                    {
                        var authorId = database.Photos.First(p => p.Author.FullName == title_user).Author.Id;

                        if (authorId != null)
                        {

                            return RedirectToAction("ResultGallery", "Photos", new { @id = authorId });
                        }
                    }
                    catch { }

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