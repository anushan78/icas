using ExamService.Data;
using System.Collections.Generic;

namespace ExamService.Repositories
{
    public interface ISubjectRepository
    {
        List<Subject> GetAll();
        Subject GetById(int Id);
        int Insert(Subject examPaper);
    }
}
