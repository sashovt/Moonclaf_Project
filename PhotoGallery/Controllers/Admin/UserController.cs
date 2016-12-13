using PhotoGallery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhotoGallery.Controllers.Admin
{
    public class UserController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: User
        public ActionResult List()
        {
            using (db)
            {
                var users = db.Users
                    .ToList();

                return View(users);
            }
        }
    }
}