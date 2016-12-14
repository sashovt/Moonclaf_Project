using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using PhotoGallery.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace PhotoGallery.Controllers.Admin
{
    [Authorize(Roles ="Admin")]
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

                var admins = GetAdminUserNames(users, db);
                ViewBag.Admins = admins;

                return View(users);
            }
        }

        private HashSet<string> GetAdminUserNames(List<ApplicationUser> users, ApplicationDbContext context)
        {
            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));

            var admins = new HashSet<string>();

            foreach(var user in users)
            {
                if(userManager.IsInRole(user.Id,"Admin"))
                {
                    admins.Add(user.UserName);
                }
            }

            return admins;
        }

        //GET: User/Edit
        public ActionResult Edit(string id)
        {
            if(id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (db)
            {
                var user = db.Users
                    .Where(u => u.Id == id)
                    .First();

                if(user==null)
                {
                    return HttpNotFound();
                }

                var viewModel = new EditUserViewModel();
                viewModel.User = user;
                viewModel.Roles = GetUserRoles(user, db);

                return View(viewModel);
            }
        }

        private List<Role> GetUserRoles(ApplicationUser user, ApplicationDbContext db)
        {
            var userManager = Request
                .GetOwinContext()
                .GetUserManager<ApplicationUserManager>();

            var roles = db.Roles
                .Select(r => r.Name)
                .OrderBy(r => r)
                .ToList();

            var userRoles = new List<Role>();

            foreach(var roleName in roles)
            {
                var role = new Role { Name = roleName };

                if(userManager.IsInRole(user.Id,roleName))
                {
                    role.IsSelected = true;
                }

                userRoles.Add(role);
            }

            return userRoles;
        }

        //POST: User/Edit
        [HttpPost]
        public ActionResult Edit(string id,EditUserViewModel viewModel)
        {
            if(ModelState.IsValid)
            {
                using (db)
                {
                    var user = db.Users.FirstOrDefault(u => u.Id == id);

                    if(user==null)
                    {
                        return HttpNotFound();
                    }

                    if(!string.IsNullOrEmpty(viewModel.Password))
                    {
                        var hasher = new PasswordHasher();
                        var passwordHash = hasher.HashPassword(viewModel.Password);
                        user.PasswordHash = passwordHash;
                    }

                    user.Email = viewModel.User.Email;
                    user.FullName = viewModel.User.FullName;
                    user.UserName = viewModel.User.Email;
                    this.SetUserRoles(viewModel, user, db);

                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("List");
                }
            }
            return View(viewModel);
        }

        private void SetUserRoles(EditUserViewModel viewModel, ApplicationUser user, ApplicationDbContext context)
        {
            var userManager = HttpContext.GetOwinContext()
                .GetUserManager<ApplicationUserManager>();

            foreach (var role in viewModel.Roles)
            {
                if(role.IsSelected && !userManager.IsInRole(user.Id,role.Name))
                {
                    userManager.AddToRole(user.Id, role.Name);
                }
                else if(!role.IsSelected && userManager.IsInRole(user.Id,role.Name))
                {
                    userManager.RemoveFromRole(user.Id, role.Name);
                }
            }
        }

        //GET: User/Delete
        public ActionResult Delete(string id)
        {
            if(id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (db)
            {
                var user = db.Users
                    .Where(u => u.Id.Equals(id))
                    .First();

                if(user==null)
                {
                    return HttpNotFound();
                }

                return View(user);
            }
        }

        //POST:User/Delete
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            if(id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (db)
            {
                var user = db.Users
                    .Where(u => u.Id.Equals(id))
                    .First();

                var userPhotos = db.Photos
                    .Where(a => a.Author.Id == user.Id);

                foreach(var photo in userPhotos)
                {
                    db.Photos.Remove(photo);
                }

                db.Users.Remove(user);
                db.SaveChanges();

                return RedirectToAction("List");
            }
        }
    }
}