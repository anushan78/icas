using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IcasDrive.Models;

namespace IcasDrive.Controllers
{
    public class PaperCartController : ControllerBase
    {
        // GET: PaperCart
        public ActionResult Index()
        {
            var paperCartViewModel = new PaperCartViewModel();
            List<SelectListItem> gradesListItems = getGradesList();

            paperCartViewModel.Grades = gradesListItems;

            return View(paperCartViewModel);
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

        [HttpPost]
        public ActionResult GetExams(PaperCartViewModel model)
        {
            var apiUrl = string.Format("exam/{0}/forgrade", model.SelectedGrade);
            var examList = HttpDataProvider.GetData<List<dynamic>>(apiUrl);
            model.GradePapers = new List<GradeExamPaper>();

            // Todo: Add this into cache
            model.Grades = getGradesList();

            examList.ForEach(delegate (dynamic exam)
            {
                model.GradePapers.Add(new GradeExamPaper { FileStoreId = exam.FileStoreId, PaperName = exam.PaperName, PaperId = exam.Id });
            });

            return View("Index", model);
        }
    }
}