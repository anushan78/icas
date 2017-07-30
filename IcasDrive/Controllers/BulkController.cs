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
                    Session["GoogleDriveFolders"] = fileList;

                    bulkViewModel.Folders = getDriveFoldersList(fileList);
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
        public async Task<ActionResult> GetDrivePapersByFolderAsync(CancellationToken cancellationToken, BulkViewModel model)
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
                    var request = service.Files.List();
                    request.Q = string.Format("mimeType = 'application/pdf' and '{0}' in parents", model.SelectedFolder);
                    var fileList = await request.ExecuteAsync();
                    var drivePaperListItems = new List<SelectListItem>();

                    foreach (var file in fileList.Items)
                    {
                        drivePaperListItems.Add(new SelectListItem { Value = file.Id, Text = file.Title });
                    }

                    model.Folders = getDriveFoldersList((FileList)Session["GoogleDriveFolders"]);
                    model.Grades = getGradesList();
                    model.Subjects = getSubjectsList();
                    model.DrivePapers = drivePaperListItems;

                }
                catch (Exception ex)
                {
                    // Todo: Log errors and show friendly error
                }

                return View("Index", model);
            }
            else
            {
                return new RedirectResult(result.RedirectUri);
            }
        }

        private List<SelectListItem> getGradesList()
        {
            var gradesListItems = new List<SelectListItem>();

            if (Session["Grades"] == null)
            {
                Session["Grades"] = HttpDataProvider.GetData<List<dynamic>>("grade/all");
            }

            ((List<dynamic>)Session["Grades"]).ForEach(delegate (dynamic grade)
            {
                gradesListItems.Add(new SelectListItem { Value = grade.Id, Text = grade.GradeName });
            });

            return gradesListItems;
        }

        private List<SelectListItem> getSubjectsList()
        {
            var subjectsListItems = new List<SelectListItem>();

            if (Session["Subjects"] == null)
            {
                Session["Subjects"] = HttpDataProvider.GetData<List<dynamic>>("subject/all");
            }

            ((List<dynamic>)Session["Subjects"]).ForEach(delegate (dynamic subject)
            {
                subjectsListItems.Add(new SelectListItem { Value = subject.Id, Text = subject.SubjectName });
            });

            return subjectsListItems;
        }

        private List<SelectListItem> getDriveFoldersList(FileList fileList)
        {
            var folderListItems = new List<SelectListItem>();

            foreach (var file in fileList.Items)
            {
                folderListItems.Add(new SelectListItem { Value = file.Id, Text = file.Title });
            }

            return folderListItems;
        }
    }
}