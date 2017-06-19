using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IcasDrive.Models
{
    public class PaperCartViewModel
    {
        public IEnumerable<SelectListItem> Grades { get; set; }
        public int SelectedGrade { get; set; }
        public List<GradeExamPaper> GradePapers { get; set; } 
    }

    public class GradeExamPaper
    {
        public string PaperName { get; set; }
        public string FileStoreId { get; set; }
    }
}