using ExamService.Models;
using ExamService.Services;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;

namespace ExamService.Controllers
{
    /// <summary>
    /// API for Exam paper operations
    /// </summary>
    [RoutePrefix("api/exam")]
    public class ExamController : ApiController
    {
        private IIcasExamService ExamService { get; set; }

        public ExamController(IIcasExamService examService)
        {
            ExamService = examService;
        }

        /// <summary>
        /// Return all Exam papers
        /// </summary>
        /// <returns>All Exam papers</returns>
        [Route("all")]
        [ResponseType(typeof(IList<ExamPaperDetails>))]
        public IHttpActionResult GetAll()
        {
            var examList = ExamService.GetAll();
            var result = (examList != null) ? (IHttpActionResult)Ok(examList) : NotFound();

            return result;
        }

        [Route("{gradeId:int}/forgrade")]
        [ResponseType(typeof(IList<ExamPaperDetails>))]
        public IHttpActionResult GetByGrade(int gradeId)
        {
            var examList = ExamService.GetByGrade(gradeId);
            var result = (examList != null) ? (IHttpActionResult)Ok(examList) : NotFound();

            return result;
        }

        /// <summary>
        /// Create new exam paper
        /// </summary>
        /// <param name="examPaperDetails"></param>
        /// <returns>Id of the newly created exam paper</returns>
        [Route("create")]
        [HttpPost]
        public IHttpActionResult PostExam(ExamPaperDetails examPaperDetails)
        {
            IHttpActionResult result;

            if (ModelState.IsValid)
            {
                int examPaperId = ExamService.Create(examPaperDetails);
                result = Ok(examPaperId);
            }
            else
            {
                result = BadRequest(ModelState);
            }

            return result;
        }

        /// <summary>
        /// Create list of new exam papers
        /// </summary>
        /// <param name="examPaperList"></param>
        /// <returns></returns>
        [Route("createlist")]
        [HttpPost]
        [ResponseType(typeof(IList<int>))]
        public IHttpActionResult PostExams([FromBody]IList<ExamPaperDetails> examPaperList)
        {
            IHttpActionResult result;
            List<int> examIds = new List<int>();
            int examId;

            if (ModelState.IsValid)
            {
                foreach (var examdetail in examPaperList)
                {
                    examId = ExamService.Create(examdetail);
                    examIds.Add(examId);
                }

                result = Ok(examIds);
            }
            else
            {
                result = BadRequest(ModelState);
            }

            return result;
        }

        [Route("forids")]
        [ResponseType(typeof(IList<ExamPaperDetails>))]
        public IHttpActionResult GetByIds(string examIds)
        {
            string[] ids = examIds.Split(',');
            int[] intIds = Array.ConvertAll(ids, item => int.Parse(item));
            List<int> intList = new List<int>(intIds);
            
            var examList = ExamService.GetByIds(intList);
            var result = (examList != null) ? (IHttpActionResult)Ok(examList) : NotFound();

            return result;
        }
    }
}
