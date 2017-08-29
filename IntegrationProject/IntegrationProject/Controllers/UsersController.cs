using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IntegrationProject.Models;
using Microsoft.AspNet.Identity;

namespace IntegrationProject.Controllers
{
    public class UsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Users
        public ActionResult Index()
        {
            var user = db.User.ToList();

            return View(user);
        }

        public ActionResult Calendar()
        {
            //Query list of events and use JSONresult in viewbag or view data.
            //Make an array[] of objects

            ViewBag.Json = GetEvents();
            //ViewBag.Json = 7; 

            return View();
        }

        public JsonResult GetEvents()
        {
            //get current user login id
            //query events table. add to viewbag volunteer events
            //query events table. add to viewbag host events (keep these separate)

            var eventList = new List<object>();
            
            eventList.Add( new {
                id = "5",
                title = "JSON Event",
                start = "8/1/2017 12:00:00 AM",
                end = "8/1/2017 12:00:00 PM"
            });

            return Json(eventList);
        }

        public ActionResult ReadOnlyIndex(int? id)
        {
            var user = db.User.Where(x => x.ID == id);

            return View(user);
        }

        public ActionResult UserSignedInIndex()
        {
            var loggedUser = User.Identity.GetUserName();

            var users =
                from u in db.Users
                join v in db.User on u.UserID equals v.ID
                where u.UserName == loggedUser && u.UserID == v.ID
                select v;

            return View(users);
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //User user = db.User.Find(id);
            var users = db.User.Single(m => m.ID == id);

            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            var loggedUser = User.Identity.GetUserName();
            var users = db.User.Single(c => c.Email == loggedUser);

            return View(users);
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                //db.User.Add(user);
                var loggedUser = User.Identity.GetUserName();
                var users = db.User.Single(c => c.Email == loggedUser);

                users.Phone = user.Phone;
                users.Description = user.Description;

                db.Entry(users).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("UserSignedInIndex");
            }

            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FirstName,LastName,Email,Phone,Description,VolunteerUpVotes,VolunteerDownVotes,EventUpVotes,EventDownVotes,NoShowCount")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("UserSignedInIndex");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.User.Find(id);
            db.User.Remove(user);
            db.SaveChanges();
            return RedirectToAction("UserSignedInIndex");
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
