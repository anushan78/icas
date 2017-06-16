using ExamService.Data;
using System.Collections.Generic;

namespace ExamService.Repositories
{
    public interface IGradeRepository
    {
        List<Grade> GetAll();
        Grade GetById(int Id);
        int Insert(Grade examPaper);
    }
}
