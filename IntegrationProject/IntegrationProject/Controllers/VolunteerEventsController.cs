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
    public class VolunteerEventsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: VolunteerEvents
        public ActionResult Index(string searchString)
        {
            var events = from m in db.VolunteerEvent
                         select m;



            if (!String.IsNullOrEmpty(searchString))
            {
                events = events.Where(s => s.Address.Contains(searchString));
            }

            return View(events);
            //return View(db.VolunteerEvent.ToList());

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
        public ActionResult Create([Bind(Include = "ID,EventName,HostID,Description,Address,City,State,Zip,StartDate,EndDate,AllDay")] VolunteerEvent volunteerEvent)
        {
            volunteerEvent.HostID = User.Identity.GetUserId();
            //TODO: Add action to update user profile calendar

            if (ModelState.IsValid)
            {
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
    }
}
