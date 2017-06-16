using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;

using IcasDrive.Models;
using IcasDrive.Core;

namespace IcasDrive.Controllers
{
    public class ExamGradeController : Controller
    {
        public ExamGradeController()
        {
            HttpDataProvider = new HttpDataProvider(ConfigurationManager.AppSettings["ExamApiBaseUrl"]);
            ApplicationName = ConfigurationManager.AppSettings["ApplicationName"];
        }

        // GET: ExamGrade
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SaveGrade(GradeViewModel model)
        {
            try
            {
                var gradeDetails = new { GradeName = model.GradeName };
                var saveGradeResponse = HttpDataProvider.PostAndReturn<dynamic, dynamic>("grade/create", gradeDetails);
            }
            catch (Exception ex)
            {

            }

            return View("Index", model);
        }

        private HttpDataProvider HttpDataProvider { get; set; }

        private string ApplicationName { get; set; }
    }
}