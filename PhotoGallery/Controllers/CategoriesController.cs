using PhotoGallery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhotoGallery.Controllers
{
    public class CategoriesController : Controller
    {
        // GET: Category
        public ActionResult Index()
        {
            using (var database = new ApplicationDbContext())
            {
                var categories = database.Categories
                    .ToList();

                return View(categories);
            }
        }
    }
}