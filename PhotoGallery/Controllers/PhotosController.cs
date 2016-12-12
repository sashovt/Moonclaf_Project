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
    public class PhotosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
       

        // GET: Photos/Gallery
        public ActionResult Gallery()
        {
            ViewBag.Message = "Gallery";    
            using (db)
            {
                var photos = db.Photos
                    .Include(a => a.Author)
                    .ToList();

                return View(photos);
            }  
        }
        public ActionResult newGallery()
        {

            ViewBag.Message = "Gallery";
            using (db)
            {
                var photos = db.Photos
                    .Include(a => a.Author)
                    .ToList();

                return View(photos);
            }
        }
        // GET: Photos/Details/5
        public ActionResult Details(int? id)
        {
            ViewBag.Message = "Other";
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

                return View(photo);
            }
        }

        // GET: Photos/Create
        [Authorize]
        public ActionResult Create()
        {
            var photo= new Photo();
            return View(photo);
        }

        // POST: Photos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "Id,Title,DateAdded")] Photo photo,HttpPostedFileBase file)
        {
            ViewBag.Message = "PostImage";

            var authorId = db.Users
                .Where(u => u.UserName == this.User.Identity.Name)
                .First()
                .Id;

            if (file != null)
            {
                photo.Image = new byte[file.ContentLength];
                file.InputStream.Read(photo.Image, 0, file.ContentLength);
            }

            photo.AuthorId = authorId;

            db.Photos.Add(photo);
            db.SaveChanges();

            return RedirectToAction("MyGallery","Photos");
        }

        // GET: Photos/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

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

            using (db)
            {
                var photo = db.Photos
                    .Where(p => p.Id == id)
                    .Include(a => a.Author)
                    .First();

                if(!IsUserAuthorizedToEdit(photo))
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
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (db)
            {
                var photo = db.Photos
                    .Where(p => p.Id == id)
                    .Include(a => a.Author)
                    .First();

                if(photo == null)
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
            ViewBag.Message = "MyGallery";
            return View(db.Photos.Where(u => u.Author.Email == User.Identity.Name).ToList());
        }
      
        public ActionResult ResultGallery(string userName)
        {
            
            ViewBag.Message = "Home";
            return View(db.Photos.Where(u => u.Author.Id == userName).ToList()); 
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
            bool isAuthor = photo.IsAuthor(this.User.Identity.Name);

            return isAuthor;
        }
    }
}
