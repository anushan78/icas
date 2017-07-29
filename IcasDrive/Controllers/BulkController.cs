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
    public class BulkController : ControllerBase
    {
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
                    var bulkViewModel = new BulkViewModel();

                    FilesResource.ListRequest request = service.Files.List();
                    request.Q = "mimeType = 'application/vnd.google-apps.folder'";

                    var fileList = await request.ExecuteAsync();
                    var folderListItems = new List<SelectListItem>();

                    foreach (var file in fileList.Items)
                    {
                        folderListItems.Add(new SelectListItem { Value = file.Id, Text = file.Title });
                    }

                    bulkViewModel.Folders = folderListItems;
                    bulkViewModel.Grades = getGradesList();
                    bulkViewModel.Subjects = getSubjectsList();

                    return View(bulkViewModel);
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
        public ActionResult GetDrivePapersByFolder(BulkViewModel model)
        {
            return View();
        }

        private static List<SelectListItem> getGradesList()
        {
            var grades = HttpDataProvider.GetData<List<dynamic>>("grade/all");
            var gradesListItems = new List<SelectListItem>();

            grades.ForEach(delegate (dynamic grade)
            {
                gradesListItems.Add(new SelectListItem { Value = grade.Id, Text = grade.GradeName });
            });

            return gradesListItems;
        }

        private static List<SelectListItem> getSubjectsList()
        {
            var subjects = HttpDataProvider.GetData<List<dynamic>>("subject/all");
            var subjectsListItems = new List<SelectListItem>();

            subjects.ForEach(delegate (dynamic subject)
            {
                subjectsListItems.Add(new SelectListItem { Value = subject.Id, Text = subject.SubjectName });
            });

            return subjectsListItems;
        }
    }
}