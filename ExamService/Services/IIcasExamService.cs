using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ExamService.Models;

namespace ExamService.Services
{
    public interface IIcasExamService
    {
        List<ExamPaperDetails> GetAll();
        List<ExamPaperDetails> GetByGrade(int gradeId);
        int Create(ExamPaperDetails examPaperDetails);
        void Update(ExamPaperDetails examPaperDetails);
        void Delete(int examPaperId);
    }
}