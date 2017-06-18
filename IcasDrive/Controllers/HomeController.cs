using Google.Apis.Auth.OAuth2.Mvc;
using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using Google.Apis.Requests;
using Google.Apis.Services;
using IcasDrive.Core;
using IcasDrive.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;

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

        public ActionResult SelectPapers()
        {
            return View();
        }

        public async Task<ActionResult> SelectPapersAsync(CancellationToken cancellationToken, SubjectViewModel model)
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
                    var request = new BatchRequest(service);
                    string[] fileIds = new string[] { "0ByOpc19oJzbjNGstb2xtdDhJeGc", "0ByOpc19oJzbjZzY4a1hub2ZRRzg" };
                    List<string> fileUrls = new List<string>();

                    for (int i = 0; i < 2; i++)
                    {
                        request.Queue<File>(service.Files.Get(fileIds[i]),
                            (file, error, x, message) =>
                            {
                                if (error != null)
                                {

                                }
                                else
                                {
                                    fileUrls.Add(file.WebContentLink);
                                }
                            });
                    }

                    await request.ExecuteAsync();
                }
                catch (Exception ex)
                {
                    // Todo: Log errors and show friendly error
                }

                return View("SelectPapers");
            }
            else
            {
                return new RedirectResult(result.RedirectUri);
            }
        }

        public ActionResult Grade()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Grade(GradeViewModel model)
        {
            try
            {
                var gradeDetails = new { GradeName = model.GradeName };
                var saveGradeResponse = HttpDataProvider.PostAndReturn<dynamic, dynamic>("grade/create", gradeDetails);
            }
            catch (Exception ex)
            {

            }

            return View(model);
        }

        public ActionResult Subject()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Subject(SubjectViewModel model)
        {
            try
            {
                var subjectDetails = new { SubjectName = model.SubjectName };
                var saveGradeResponse = HttpDataProvider.PostAndReturn<dynamic, dynamic>("subject/create", subjectDetails);
            }
            catch (Exception ex)
            {

            }

            return View(model);
        }

        private ExamPaperViewModel assignGradesAndSubjectsListToModel(ExamPaperViewModel examPaperViewModel)
        {
            var grades = HttpDataProvider.GetData<List<dynamic>>("grade/all");
            var gradesListItems = new List<SelectListItem>();

            grades.ForEach(delegate (dynamic grade)
            {
                gradesListItems.Add(new SelectListItem { Value = grade.Id, Text = grade.GradeName });
            });

            examPaperViewModel.Grades = gradesListItems;

            var subjects = HttpDataProvider.GetData<List<dynamic>>("subject/all");
            var subjectsListItems = new List<SelectListItem>();

            subjects.ForEach(delegate (dynamic subject)
            {
                subjectsListItems.Add(new SelectListItem { Value = subject.Id, Text = subject.SubjectName });
            });

            examPaperViewModel.Subjects = subjectsListItems;

            return examPaperViewModel;
        }

        private HttpDataProvider HttpDataProvider { get; set; }

        private string ApplicationName { get; set; }
    }
}
