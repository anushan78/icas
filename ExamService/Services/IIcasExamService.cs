using ExamService.Models;
using System.Collections.Generic;

namespace ExamService.Services
{
    public interface IIcasExamService
    {
        List<ExamPaperDetails> GetAll();
        List<ExamPaperDetails> GetByGrade(int gradeId);
        int Create(ExamPaperDetails examPaperDetails);
        List<ExamPaperDetails> GetByIds(List<int> ids);
        void Update(ExamPaperDetails examPaperDetails);
        void Delete(int examPaperId);
    }
}