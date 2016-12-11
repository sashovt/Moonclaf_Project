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
            Photo photo = db.Photos.Find(id);
            if (photo == null)
            {
                return HttpNotFound();
            }
            
            return View(photo);
        }

        // GET: Photos/Create
        [Authorize]
        public ActionResult Create()
        {
             Photo photo = new Photo();
            ViewBag.Message = "PostImage";
          
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
            if (file != null)
            {
               photo.Image = new byte[file.ContentLength];
                file.InputStream.Read(photo.Image, 0, file.ContentLength);
            }

            photo.Author = db.Users.FirstOrDefault(u => u.Email == User.Identity.Name);

            db.Photos.Add(photo);
            db.SaveChanges();
            
            return RedirectToAction("MyGallery","Photos");
        }

        // GET: Photos/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            ViewBag.Message = "Other";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Photo photo = db.Photos.Find(id);

            //if (!IsUserAuthorizedToEdit(photo))
            //{
<<<<<<< HEAD
            // return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
=======
               // return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
>>>>>>> 1245cf809a1263c5626fbb23fe26cad8a6535df1
            //}

            if (photo == null)
            {
                return HttpNotFound();
            }
            return View(photo);
        }

        // POST: Photos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Photo model)
        {
<<<<<<< HEAD
            ViewBag.Message = "Other";
            using (var database = new ApplicationDbContext())
            {
                var photo = database.Photos
                    .FirstOrDefault(p => p.Id == model.Id);

                photo.Title = model.Title;
                DateTime currentDate = DateTime.Now;
                photo.DateAdded = currentDate;

                database.Entry(photo).State = EntityState.Modified;
                database.SaveChanges();

                return RedirectToAction("MyGallery", "Photos");
            }
=======
                using (var database = new ApplicationDbContext())
                {
                    var photo = database.Photos
                        .FirstOrDefault(p => p.Id == model.Id);

                    photo.Title = model.Title;
                    DateTime currentDate=DateTime.Now;
                    photo.DateAdded = currentDate;

                    database.Entry(photo).State = EntityState.Modified;
                    database.SaveChanges();

                    return RedirectToAction("MyGallery", "Photos");
                }
>>>>>>> 1245cf809a1263c5626fbb23fe26cad8a6535df1
        }

        // GET: Photos/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            ViewBag.Message = "Other";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Photo photo = db.Photos.Find(id);

            //if (!IsUserAuthorizedToEdit(photo))
            //{
                //return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            //}

            if (photo == null)
            {
                return HttpNotFound();
            }
            return View(photo);
        }

        // POST: Photos/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ViewBag.Message = "Other";
            Photo photo = db.Photos.Find(id);
            db.Photos.Remove(photo);
            db.SaveChanges();
            return RedirectToAction("MyGallery", "Photos");
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

        //private bool IsUserAuthorizedToEdit(Photo photo)
        //{
            //bool isAuthor = photo.IsAuthor(this.User.Identity.Name);
            //return isAuthor;
        //}
    }
}
