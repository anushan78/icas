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
    }
}