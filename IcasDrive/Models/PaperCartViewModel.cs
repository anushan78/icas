﻿using System;
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
        public string SelectedPapers { get; set; }
    }

    public class GradeExamPaper
    {
        public int PaperId { get; set; }
        public string PaperName { get; set; }
    }
}