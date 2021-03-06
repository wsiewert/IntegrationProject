﻿using System;
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

            //ViewBag.Json = GetEvents();
            //ViewBag.Json = 7;
            ViewBag.Json = "[{ title: JSON EV, start: \"8/1/2017 12:00:00 AM\", end: \"8/1/2017 12:00:00 PM\" }]";

            return View();
        }

        public JsonResult GetEvents()
        {
            //get current user login id
            //query events table. add to viewbag volunteer events
            //query events table. add to viewbag host events (keep these separate)

            var eventList = new List<object>();

            eventList.Add( new 
            {
                title = "JSON Event",
                start = "8/1/2017 12:00:00 AM",
                end = "8/1/2017 12:00:00 PM"
            });

            return Json(eventList, JsonRequestBehavior.AllowGet);
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
        public ActionResult Edit(User user)
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

        public ActionResult IndexEventVolunteers(int id)
        {
            ViewBag.VolunteerEventId = id;

            var loggedUser = User.Identity.GetUserName();
            var usersId = db.User.Single(v => v.Email == loggedUser);

            var users =
                (from u in db.User_Event
                 join e in db.VolunteerEvent on u.VolunteerEventID equals e.ID
                 join v in db.User on u.UserID equals v.ID
                 where u.VolunteerEventID == id && u.UserID != usersId.ID
                 select v);

            return View(users);
        }

        public ActionResult IndexEventVolunteersViewOnly(int id)
        {
            var loggedUser = User.Identity.GetUserName();
            var usersId = db.User.Single(v => v.Email == loggedUser);

            var users =
                (from u in db.User_Event
                 join e in db.VolunteerEvent on u.VolunteerEventID equals e.ID
                 join v in db.User on u.UserID equals v.ID
                 where u.VolunteerEventID == id && u.UserID != usersId.ID
                 select v);

            return View(users);
        }

        public ActionResult IndexEventHost(int id)
        {
            ViewBag.HostEventId = id;

            var host = (from a in db.Users
                        join e in db.VolunteerEvent on a.Id equals e.HostID
                        join v in db.User on a.UserID equals v.ID
                        where e.ID == id && e.HostID == a.Id && a.UserID == v.ID
                        select v);

            return View(host);
        }

        public ActionResult VolunteerDownVotesCounter(int id, int eventId)
        {           
            var rated = db.User_Event.Single(u => u.UserID == id && u.VolunteerEventID == eventId);

            if (rated.Rated == false)
            {
                rated.Rated = true;

                var users = db.User.Single(m => m.ID == id);
                users.VolunteerDownVotes++;

                db.Entry(rated).State = EntityState.Modified;
                db.Entry(users).State = EntityState.Modified;
                db.SaveChanges();
            }

            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult VolunteerUpVotesCounter(int id, int eventId)
        {
            var rated = db.User_Event.Single(u => u.UserID == id && u.VolunteerEventID == eventId);

            if (rated.Rated == false)
            {
                rated.Rated = true;

                var users = db.User.Single(m => m.ID == id);
                users.VolunteerUpVotes++;

                db.Entry(rated).State = EntityState.Modified;
                db.Entry(users).State = EntityState.Modified;
                db.SaveChanges();                
            }

            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult EventUpVotesCounter(int id, int eventId)
        {
            var rated = db.User_Event.Single(u => u.UserID == id && u.VolunteerEventID == eventId);

            if (rated.Rated == false)
            {
                rated.Rated = true;

                var users = db.User.Single(m => m.ID == id);
                users.EventUpVotes++;

                db.Entry(rated).State = EntityState.Modified;
                db.Entry(users).State = EntityState.Modified;
                db.SaveChanges();
            }

            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult EventDownVotesCounter(int id, int eventId)
        {
            var rated = db.User_Event.Single(u => u.UserID == id && u.VolunteerEventID == eventId);

            if (rated.Rated == false)
            {
                rated.Rated = true;

                var users = db.User.Single(m => m.ID == id);
                users.EventDownVotes++;

                db.Entry(rated).State = EntityState.Modified;
                db.Entry(users).State = EntityState.Modified;
                db.SaveChanges();
            }

            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult NoShowVotesCounter(int id, int eventId)
        {
            var rated = db.User_Event.Single(u => u.UserID == id && u.VolunteerEventID == eventId);

            if (rated.Rated == false)
            {
                rated.Rated = true;

                var users = db.User.Single(m => m.ID == id);
                users.NoShowCount++;

                db.Entry(rated).State = EntityState.Modified;
                db.Entry(users).State = EntityState.Modified;
                db.SaveChanges();
            }
            return Redirect(Request.UrlReferrer.ToString());
        }

    }
}
