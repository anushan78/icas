﻿using Google.Apis.Auth.OAuth2.Mvc;
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
using System.Configuration;

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
            var emailTemplate = System.IO.File.ReadAllText(Server.MapPath(ConfigurationManager.AppSettings["EmailTemplatePath"]));
            var body = Engine.Razor.RunCompile(emailTemplate, "TemplateKey", typeof(SelectionViewModel), model);

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(ConfigurationManager.AppSettings["EmailFrom"]);
                mail.To.Add(model.EmailAddress);
                mail.Subject = string.Format("ICAS Papers {0}", model.CustomerName);
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

            return View("Index", model);
        }
    }
}
