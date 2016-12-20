using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PhotoGallery.Models;
using System.Drawing;
using System.IO;
namespace PhotoGallery.Controllers
{
    public static class back
    {
        public static int backIndex { get; set; }

        public static int cancel { get; set; }
    }
    public class PhotosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        // GET: Photos/Gallery
        public ActionResult Gallery()
        { 
            back.backIndex = 1;
            back.cancel = 1;
            using (db)
            {
                var photo = new GalleryViewModel(db.Photos.Include(a => a.Author).ToList(), db.Categories.OrderBy(c => c.Name).ToList());
                return View(photo);
            }
        }

        // GET: Photos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.backIndex = back.backIndex;

            using (db)
            {
                var photo = db.Photos
                    .Where(p => p.Id == id)
                    .Include(a => a.Author)
                    .Include(c => c.Category)
                    .First();

                if (photo == null)
                {
                    return HttpNotFound();
                }

                return View(photo);
            }
        }

        // GET: Photos/Create
        [Authorize]
        public ActionResult Create()
        {

            using (db)
            {
                var photo = new PhotoViewModel();
                photo.Categories = db.Categories

                    .OrderBy(c => c.Name)
                    .ToList();

                return View(photo);
            }

        }

        // POST: Photos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        public ActionResult Create(PhotoViewModel photo, HttpPostedFileBase file)
        {

            var authorId = db.Users
                .Where(u => u.UserName == this.User.Identity.Name)
                .First()
                .Id;

            if (file != null)
            {
                photo.Image = new byte[file.ContentLength];
                file.InputStream.Read(photo.Image, 0, file.ContentLength);
            }

            var image = new Photo(authorId, photo.Title, photo.Image, photo.CategoryId, photo.DateAdded);

            db.Photos.Add(image);
            db.SaveChanges();


            return RedirectToAction("MyGallery", "Photos");
        }

        // GET: Photos/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.cancel= back.cancel;

            using (db)
            {
                var photo = db.Photos
                    .Where(p => p.Id == id)
                    .First();

                if (!IsUserAuthorizedToEdit(photo))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                if (photo == null)
                {
                    return HttpNotFound();
                }

                var model = new PhotoViewModel();
                model.Id = photo.Id;
                model.AuthorId = photo.AuthorId;
                model.Title = photo.Title;
                model.Image = photo.Image;
                DateTime currentDate = DateTime.Now;
                model.DateAdded = currentDate;
                model.CategoryId = photo.CategoryId;
                model.Categories = db.Categories
                    .OrderBy(c => c.Name)
                    .ToList();

                return View(model);
            }
        }

        // POST: Photos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        public ActionResult Edit(PhotoViewModel model)
        {
            using (db)
            {
                var photo = db.Photos
                    .FirstOrDefault(p => p.Id == model.Id);

                photo.AuthorId = model.AuthorId;
                photo.Title = model.Title;
                photo.Image = model.Image;
                photo.DateAdded = model.DateAdded;
                photo.CategoryId = model.CategoryId;


                db.Entry(photo).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("MyGallery", "Photos");
            }
        }

        // GET: Photos/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.cancel = back.cancel;

            using (db)
            {
                var photo = db.Photos
                    .Where(p => p.Id == id)
                    .Include(a => a.Author)
                    .First();

                if (!IsUserAuthorizedToEdit(photo))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                if (photo == null)
                {
                    return HttpNotFound();
                }

                return View(photo);
            }
        }

        // POST: Photos/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (db)
            {
                var photo = db.Photos
                    .Where(p => p.Id == id)
                    .Include(a => a.Author)
                    .First();

                if (photo == null)
                {
                    return HttpNotFound();
                }

                db.Photos.Remove(photo);
                db.SaveChanges();

                return RedirectToAction("MyGallery", "Photos");
            }
        }

        // GET: Photos/MyGallery
        [Authorize]
        public ActionResult MyGallery()
        {
            back.backIndex = 2;
            back.cancel = 2;
            var photo = new GalleryViewModel(db.Photos.Where(u => u.Author.Email == User.Identity.Name).ToList(), db.Categories.OrderBy(c => c.Name).ToList());
            return View(photo);
        }
        [AllowAnonymous]
        public ActionResult ResultGallery(string id)
        {
            using (db)
            {
                try
                {
                    back.backIndex = 4;
                    back.cancel = 4;
                    var photo = new GalleryViewModel(db.Photos.Where(a => a.Author.Id == id).ToList(), db.Categories.OrderBy(c => c.Name).ToList());
                    ViewBag.User = db.Photos.FirstOrDefault(a => a.Author.Id == id).Author.FullName;

                    return View(photo);
                }
                catch
                {
                    return RedirectToAction("Index", "Home");
                }


            }
        }


        //GET:Photos/GategoryGallery/5
        public ActionResult CategoryGallery(int? id)
        {
            back.backIndex = 3;
            back.cancel = 3;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var db = new ApplicationDbContext())
            {
                var photo = new GalleryViewModel(db.Photos.Where(c => c.CategoryId == id).ToList(), db.Categories.OrderBy(c => c.Name).ToList());
                ViewBag.Category = db.Categories.FirstOrDefault(c => c.Id == id).Name;
                return View(photo);
               
            }
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public bool IsUserAuthorizedToEdit(Photo photo)
        {
            bool isAdmin = this.User.IsInRole("Admin");
            bool isAuthor = photo.IsAuthor(this.User.Identity.Name);

            return isAuthor || isAdmin;
        }
    }
}
