using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IcasDrive.Models;

using System.Net;
using System.Net.Mail;
using RazorEngine.Text;
using RazorEngine.Configuration;
using RazorEngine;
using RazorEngine.Templating;
using System.IO;

namespace IcasDrive.Controllers
{
    public class SelectionController : ControllerBase
    {
        // GET: Selection
        public ActionResult Index()
        {
            var selectionViewModel = new SelectionViewModel();
            // Todo: fill with rows
            var examLink = new ExamLink { PaperName = "Introductory English 2013", PaperUrl = "https://drive.google.com/open?id=0ByOpc19oJzbja1NEa1VBcmZxaTQ" };
            selectionViewModel.ExamLinks = new List<ExamLink>();
            selectionViewModel.ExamLinks.Add(examLink);

            return View(selectionViewModel);
        }

        [HttpPost]
        public ActionResult SendEmail(SelectionViewModel model)
        {
            string smtpAddress = "smtp.mail.yahoo.com";
            int portNumber = 587;
            bool enableSSL = true;

            var selectionViewModel = new SelectionViewModel();
            // Todo: fill with rows
            var examnk = new ExamLink { PaperName = "Introductory English 2013", PaperUrl = "https://drive.google.com/open?id=0ByOpc19oJzbja1NEa1VBcmZxaTQ" };
            selectionViewModel.ExamLinks = new List<ExamLink>();
            selectionViewModel.ExamLinks.Add(examnk);

            // Todo: Add to config
            string emailFrom = "naomi_wilson82@yahoo.com";
            string password = "nishasaseni8209";
            string emailTo = model.EmailAddress;
            string subject = "Hello";
            string examLinkString = string.Empty;
            var emailTemplate = System.IO.File.ReadAllText(Server.MapPath("~/Templates/PaperListEmailTemplate.cshtml"));

            var body = Engine.Razor.RunCompile(emailTemplate, "templateKey", typeof(SelectionViewModel), selectionViewModel);

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(emailFrom);
                mail.To.Add(emailTo);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                // Can set to false, if you are sending pure text.

                using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                {
                    smtp.UseDefaultCredentials = false;
                    smtp.EnableSsl = enableSSL;
                    smtp.Credentials = new NetworkCredential(emailFrom, password);
                    smtp.Send(mail);
                }
            }

            return View("Index", model);
        }
    }
}
