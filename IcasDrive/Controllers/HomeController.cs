using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Mvc;
using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Mvc;
using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;

using IcasDrive.Models;
using IcasDrive.Core;

namespace IcasDrive.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
            HttpDataProvider = new HttpDataProvider(ConfigurationManager.AppSettings["ExamApiBaseUrl"]);
            ApplicationName = ConfigurationManager.AppSettings["ApplicationName"];
        }

        public ActionResult Index()
        {
            var examPaperViewModel = this.assignGradesAndSubjectsListToModel(new ExamPaperViewModel());
            return View(examPaperViewModel);
        }

        public async Task<ActionResult> UploadAsync(CancellationToken cancellationToken, ExamPaperViewModel model)
        {
            var result = await new AuthorizationCodeMvcApp(this, new AppFlowMetadata()).AuthorizeAsync(cancellationToken);

            if (result.Credential != null)
            {
                if ((TempData["isInitialPost"] != null) && (bool)TempData["isInitialPost"])
                {
                    model = (ExamPaperViewModel)TempData["examViewModel"];
                    TempData.Remove("examViewModel");
                    TempData.Remove("isInitialPost");
                }

                var service = new DriveService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = result.Credential,
                    ApplicationName = ApplicationName
                });

                var file = new File()
                        {
                            Title = model.UploadFile.FileName,
                            MimeType = model.UploadFile.ContentType
                        };

                try
                {
                    FilesResource.InsertMediaUpload uploadRequest = service.Files.Insert(file, model.UploadFile.InputStream, file.MimeType);
                    var awList = await uploadRequest.UploadAsync();
                    var uploadResponse = uploadRequest.ResponseBody;

                    var examDetails = new {
                                      SubjectId = model.SelectedSubject,
                                      GradeId = model.SelectedGrade,
                                      Year = model.Year.ToString(),
                                      HasAnswers = model.HasAnswers,
                                      FileStoreId = uploadResponse.Id
                                    };

                    var createFileResponse = HttpDataProvider.PostAndReturn<dynamic, dynamic>("exam/create", examDetails);
                }
                catch (Exception ex)
                {
                    // Todo: Log errors and show friendly error
                }

                model = assignGradesAndSubjectsListToModel(model);
                return View("Index", model);
            }
            else
            {
                TempData.Add("examViewModel", model);
                TempData.Add("isInitialPost", true);
                return new RedirectResult(result.RedirectUri);
            }
        }

        private ExamPaperViewModel assignGradesAndSubjectsListToModel(ExamPaperViewModel examPaperViewModel)
        {
             // Todo: Get Values from Db
            examPaperViewModel.Grades = new List<SelectListItem>()
            {
               new SelectListItem { Text = "Introductory", Value = "1" },
               new SelectListItem { Text = "Year 3", Value = "2" }
            };

            examPaperViewModel.Subjects = new List<SelectListItem>()
            {
                new SelectListItem { Text = "Science", Value = "1" },
                new SelectListItem { Text = "Mathematics", Value = "2" }
            };

            return examPaperViewModel;
        }

        private HttpDataProvider HttpDataProvider { get; set; }

        private string ApplicationName { get; set; }
    }
}
