using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IcasDrive.Models
{
    public class BulkViewModel
    {
        public IEnumerable<SelectListItem> Folders { get; set; }
        public IEnumerable<SelectListItem> DrivePapers { get; set; }
        public IEnumerable<SelectListItem> Grades { get; set; }
        public IEnumerable<SelectListItem> Subjects { get; set; }
        public string SelectedFolder { get; set; }
        public int SelectedDrivePaper { get; set; }
        public int SelectedGrade { get; set; }
        public int SelectedSubject { get; set; }
        public int Year { get; set; }
        public bool HasAnswers { get; set; }
        public string PaperName { get; set; }
    }
}