using ExamService.Models;
using ExamService.Services;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;


namespace ExamService.Controllers
{
    [RoutePrefix("api/drive")]
    public class DriveController : ApiController
    {
        private IDriveFolderService DriveFolderService { get; set; }

        public DriveController(IDriveFolderService driveFolderService)
        {
            DriveFolderService = driveFolderService;
        }

        [Route("allfolders")]
        [ResponseType(typeof(IList<DriveFolderDetails>))]
        public IHttpActionResult GetAllFodlers()
        {
            var drivefolders = DriveFolderService.GetAll();
            var result = (drivefolders != null) ? (IHttpActionResult)Ok(drivefolders) : NotFound();

            return result;
        }
    }
}
