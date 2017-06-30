using Google.Apis.Auth.OAuth2.Mvc;
using Google.Apis.Drive.v2;
using Google.Apis.Requests;
using Google.Apis.Services;
using IcasDrive.Core;
using IcasDrive.Models;
using RazorEngine;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace IcasDrive.Controllers
{
    public class SelectionController : ControllerBase
    {
        // GET: Selection
        public async Task<ActionResult> Index(CancellationToken cancellationToken)
        {
            var result = await new AuthorizationCodeMvcApp(this, new AppFlowMetadata()).AuthorizeAsync(cancellationToken);

            if (result.Credential != null)
            {
                var service = new DriveService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = result.Credential,
                    ApplicationName = ApplicationName
                });

                try
                {
                    var selectionViewModel = new SelectionViewModel();
                    var fileId = string.Empty;
                    var examLinksList = new List<ExamLink>();

                    var request = new BatchRequest(service);
                    var selectedPaperIds = string.Join(",", ((List<int>)Session["SelectedIds"]).ToArray());
                    var examPapers = HttpDataProvider.GetData<List<dynamic>>(string.Format("exam/forids?examIds={0}", selectedPaperIds));

                    examPapers.ForEach(delegate (dynamic examPaper) {
                        fileId = examPaper.FileStoreId;

                        request.Queue<Google.Apis.Drive.v2.Data.File>(service.Files.Get(fileId),
                            (file, error, x, message) =>
                            {
                                if (error != null) 
                                    throw new Exception("error");
                                else
                                    examLinksList.Add(new ExamLink { PaperName = examPaper.PaperName, PaperUrl = file.WebContentLink });
                            });
                    });

                    await request.ExecuteAsync();

                    selectionViewModel.ExamLinks = examLinksList;
                    return View(selectionViewModel);
                }
                catch (Exception ex)
                {
                    // Todo: Log errors and show friendly error
                    throw ex;
                }
            }
            else
            {
                return new RedirectResult(result.RedirectUri);
            }
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
