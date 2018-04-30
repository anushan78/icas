using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IcasDrive.Models;
using System.Configuration;
using RazorEngine;
using RazorEngine.Templating;
using System.Net;
using System.Net.Mail;

namespace IcasDrive.Controllers
{
    public class EmailController : Controller
    {
        // GET: Email
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(EmailViewModel model)
        {
            var emailTemplate = System.IO.File.ReadAllText(Server.MapPath(ConfigurationManager.AppSettings["EmailTemplatePath"]));
            var body = Engine.Razor.RunCompile(emailTemplate, "TemplateKey", typeof(SelectionViewModel), model);

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(ConfigurationManager.AppSettings["EmailFrom"]);
                mail.To.Add("anushan2003@yahoo.com");
                mail.Subject = string.Format("ICAS Papers {0}", "Anushan");
                mail.Body = body;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings["SmtpHost"], Convert.ToInt16(ConfigurationManager.AppSettings["SmtpPort"])))
                {
                    smtp.UseDefaultCredentials = false;
                    smtp.EnableSsl = true;
                    smtp.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["EmailFrom"], ConfigurationManager.AppSettings["EmailFromPassword"]);
                    smtp.Send(mail);
                }
            }

            return View(model);
        }
     }
}
