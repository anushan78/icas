using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IcasDrive.Models
{
    public class SelectionViewModel
    {
        public List<ExamLink> ExamLinks { get; set; }
        public string CustomerName { get; set; }
        public string EmailAddress { get; set; }
    }

    public class ExamLink
    {
        public string FileId { get; set; }
        public string PaperName { get; set; }
        public string PaperUrl { get; set; }
    }
}