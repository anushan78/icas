using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IcasDrive.Models
{
    public class ExamPaperViewModel
    {
        public IEnumerable<SelectListItem> Grades { get; set; }
        public IEnumerable<SelectListItem> Subjects { get; set; }
        public int SelectedGrade { get; set; }
        public int SelectedSubject { get; set; }
        public int Year { get; set; }
        public bool HasAnswers { get; set; }
        public HttpPostedFileBase UploadFile { get; set; }
    }
}