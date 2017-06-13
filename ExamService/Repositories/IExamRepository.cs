using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExamService.Data;

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
