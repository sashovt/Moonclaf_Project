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
            return View(db.Photos.ToList());
        }


        // GET: Photos/Details/5
        public ActionResult Details(int? id)
        {
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
            if (ModelState.IsValid)
            { 
                //return RedirectToAction("Index");
            }

          
            db.Photos.Add(photo);
            db.SaveChanges();

            return RedirectToAction("Index","Home");
        }

        // GET: Photos/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
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

        // POST: Photos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Image,DateAdded")] Photo photo)
        {
            if (ModelState.IsValid)
            {

                db.Entry(photo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("MyGallery", "Photos");
            }
            return View(photo);
        }

        // GET: Photos/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
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

        // POST: Photos/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
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
            return View(db.Photos.Where(u => u.Author.Id == User.Identity).ToList());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
