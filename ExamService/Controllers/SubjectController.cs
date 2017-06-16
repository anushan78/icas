using ExamService.Models;
using ExamService.Services;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;


namespace ExamService.Controllers
{
    [RoutePrefix("api/subject")]
    public class SubjectController : ApiController
    {
        private ISubjectService SubjectService { get; set; }

        public SubjectController(ISubjectService subjectService)
        {
            SubjectService = subjectService;
        }

        [Route("all")]
        [ResponseType(typeof(IList<SubjectDetails>))]
        public IHttpActionResult GetAll()
        {
            var subjects = SubjectService.GetAll();
            var result = (subjects != null) ? (IHttpActionResult)Ok(subjects) : NotFound();

            return result;
        }

        [Route("create")]
        [HttpPost]
        public IHttpActionResult PostSubject(SubjectDetails subjectDetails)
        {
            IHttpActionResult result;

            if (ModelState.IsValid)
            {
                int subjectId = SubjectService.Create(subjectDetails);
                result = Ok(subjectId);
            }
            else
            {
                result = BadRequest(ModelState);
            }

            return result;
        }
    }
}
