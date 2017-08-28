using Postal;
using System;
using System.Collections.Generic;
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
        public ActionResult Index()
        {
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
        public ActionResult SendMail()
        {
            dynamic email = new Email("SendEmail");
            email.to = "bryanneumann1@gmail.com";
            email.Message = "Im sorry but this event has been canceled.";
            email.Send();

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact(EmailFormModel model)
        {
            if (ModelState.IsValid)
            {
                var body = "<p>Update from Event: {0} ({1})</p><p>Message:</p><p>{2}</p>";
                //var recipients = model.volunteersSignedUp;
                var message = new MailMessage();
                //message.To.Add = string.Format(recipients);
                message.To.Add(new MailAddress("Bryanneumann1@gmail.com"));  // Sends to this address
                message.From = new MailAddress("teamintegrationproject@gmail.com");  
                message.Subject = "Important information about a volunteer event";
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