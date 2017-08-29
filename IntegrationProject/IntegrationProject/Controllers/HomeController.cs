using Postal;
using System;
using System.Data;
using System.Data.Entity;
using System.Collections.Generic;
using IntegrationProject.Models;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using MVCEmail.Models;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace IntegrationProject.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            ViewBag.LoggedUser = User.Identity.GetUserId();

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult SendMail(int? id)
        {
            ViewBag.Id = id;
                
            var recipients =
               (from u in db.User_Event
                join e in db.VolunteerEvent on u.VolunteerEventID equals e.ID
                join v in db.User on u.UserID equals v.ID
                where e.ID == id && u.VolunteerEventID == e.ID && u.UserID == v.ID
                select v);
            foreach (var v in recipients)
            {
                dynamic email = new Email("SendEmail");
                email.to = v.Email.ToString();
                email.Message = "There has been an update to a volunteer event.";
                email.Send();
            }
            return View(id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact(EmailFormModel model)
        {
            if (ModelState.IsValid)
            { 
                var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
                var message = new MailMessage();
                message.To.Add(new MailAddress("Bryanneumann1@gmail.com"));  // Sends to this address
                message.From = new MailAddress("teamintegrationproject@gmail.com");  
                message.Subject = "Customer Feedback";
                message.Body = string.Format(body, model.FromName, model.FromEmail, model.Message);
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = "teamintegrationproject@gmail.com",  // Actually uses this email to send the message
                        Password = "vanadium1"  // Uses this password with above username to send.
                    };
                    smtp.Credentials = credential;
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(message);
                    return RedirectToAction("Sent");
                }
            }
            return View(model);
        }
        public ActionResult Sent()
        {
            return View();
        }
    }
}