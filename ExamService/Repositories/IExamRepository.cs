using ExamService.Data;
using System.Collections.Generic;

namespace ExamService.Repositories
{
    public interface IExamRepository
    {
        List<ExamPaper> GetAll();
        ExamPaper GetById(int Id);
        int Insert(ExamPaper examPaper);
        void Update(ExamPaper examPaper);
        void Delete(ExamPaper examPaper);
    }
}
