using ExamService.Models;
using ExamService.Services;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;


namespace ExamService.Controllers
{
    [RoutePrefix("api/grade")]
    public class GradeController : ApiController
    {
        private IGradeService GradeService { get; set; }

        public GradeController(IGradeService gradeService)
        {
            GradeService = gradeService;
        }

        [Route("all")]
        [ResponseType(typeof(IList<GradeDetails>))]
        public IHttpActionResult GetAll()
        {
            var grades = GradeService.GetAll();
            var result = (grades != null) ? (IHttpActionResult)Ok(grades) : NotFound();

            return result;
        }

        [Route("create")]
        [HttpPost]
        public IHttpActionResult PostGrade(GradeDetails gradeDetails)
        {
            IHttpActionResult result;

            if (ModelState.IsValid)
            {
                int gradeId = GradeService.Create(gradeDetails);
                result = Ok(gradeId);
            }
            else
            {
                result = BadRequest(ModelState);
            }

            return result;
        }
    }
}
