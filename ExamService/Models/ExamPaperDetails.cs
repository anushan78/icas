using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamService.Models
{
    public class ExamPaperDetails
    {
        public int SubjectId { get; set; }
        public int GradeId { get; set; }
        public string Year { get; set; }
        public bool HasAnswers { get; set; }
        public string FileStoreId { get; set; }
    }
}