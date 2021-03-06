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
    public class VolunteerEventsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: VolunteerEvents
        public ActionResult Index(string searchString)
        {
            ViewBag.LoggedUser = User.Identity.GetUserId();

            var events = from m in db.VolunteerEvent
                         select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                events = events.Where(s => s.Address.Contains(searchString));
            }

            return View(events);
        }

        public ActionResult IndexMyEvents()
        {
            var loggedUserID = User.Identity.GetUserId();
            var loggedUser = User.Identity.GetUserName();
            var users = db.User.Single(v => v.Email == loggedUser);

            var events =
                from u in db.User_Event
                join e in db.VolunteerEvent on u.VolunteerEventID equals e.ID
                join v in db.User on u.UserID equals v.ID
                where u.UserID == users.ID
                select e;

            ViewBag.LoggedUserID = loggedUserID;
            return View(events);
        }

        // GET: VolunteerEvents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            VolunteerEvent volunteerEvent = db.VolunteerEvent.Find(id);
            var host = (from a in db.Users
                        join e in db.VolunteerEvent on a.Id equals e.HostID
                        join v in db.User on a.UserID equals v.ID
                        where e.ID == id && e.HostID == a.Id && a.UserID == v.ID
                        select new
                        {
                            FirstName = v.FirstName,
                            LastName = v.LastName,
                            HostID = e.HostID,
                            EndDate = e.EndDate
                        });

            foreach (var x in host)
            {
                ViewBag.First = x.FirstName;
                ViewBag.Last = x.LastName;
                ViewBag.Host = x.HostID;
                ViewBag.EndDate = x.EndDate;
            }

            ViewBag.LoggedUser = User.Identity.GetUserId();

            var loggedUser = User.Identity.GetUserName();
            var users = db.User.Single(v => v.Email == loggedUser);

            var userEvents =
                from u in db.User_Event
                join e in db.VolunteerEvent on u.VolunteerEventID equals e.ID
                join v in db.User on u.UserID equals v.ID
                where u.UserID == users.ID && u.VolunteerEventID == id
                select v;

            if(!userEvents.Any())
            {
                ViewBag.Volunteer = "false";
            }

            if (volunteerEvent == null)
            {
                return HttpNotFound();
            }
            return View(volunteerEvent);
        }

        // GET: VolunteerEvents/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: VolunteerEvents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VolunteerEvent volunteerEvent)
        {
            var loggedUser = User.Identity.GetUserName();
            var users = db.User.Single(v => v.Email == loggedUser);

            volunteerEvent.HostID = User.Identity.GetUserId();

            User_Event user_event = new User_Event
            {
                UserID = users.ID,
                VolunteerEventID = volunteerEvent.ID
            };

            if (ModelState.IsValid)
            {
                db.User_Event.Add(user_event);
                db.VolunteerEvent.Add(volunteerEvent);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(volunteerEvent);
        }

        // GET: VolunteerEvents/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VolunteerEvent volunteerEvent = db.VolunteerEvent.Find(id);
            if (volunteerEvent == null)
            {
                return HttpNotFound();
            }
            return View(volunteerEvent);
        }

        // POST: VolunteerEvents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit(VolunteerEvent volunteerEvent)
        {
            if (ModelState.IsValid)
            {
                db.Entry(volunteerEvent).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("SendMail", "Home", volunteerEvent);
            }
            return View(volunteerEvent);
        }

        // GET: VolunteerEvents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VolunteerEvent volunteerEvent = db.VolunteerEvent.Find(id);
            if (volunteerEvent == null)
            {
                return HttpNotFound();
            }
            return View(volunteerEvent);
        }

        // POST: VolunteerEvents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VolunteerEvent volunteerEvent = db.VolunteerEvent.Find(id);
            db.VolunteerEvent.Remove(volunteerEvent);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult IndexVolunteerRoute(int id)
        {
            //return View();
            return RedirectToAction("IndexEventVolunteers", "Users", new { id = id });
        }

        public ActionResult IndexVolunteerRouteViewOnly(int id)
        {
            //return View();
            return RedirectToAction("IndexEventVolunteersViewOnly", "Users", new { id = id });
        }

        public ActionResult IndexHostRoute(int id)
        {
            //return View();
            return RedirectToAction("IndexEventHost", "Users", new { id = id });
        }

        public ActionResult AddUserToEvent(int id)
        {
            var loggedUser = User.Identity.GetUserName();
            var users = db.User.Single(v => v.Email == loggedUser);

            User_Event user_event = new User_Event
            {
                UserID = users.ID,
                VolunteerEventID = id
            };

            db.User_Event.Add(user_event);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult CancelVolunteerEvent(int id)
        {
            var loggedUser = User.Identity.GetUserName();
            var users = db.User.Single(v => v.Email == loggedUser);

            var userEvent = db.User_Event.Single(e => e.VolunteerEventID == id && e.UserID == users.ID);
           
            User_Event user_event = db.User_Event.Find(userEvent.ID);
            db.User_Event.Remove(userEvent);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
