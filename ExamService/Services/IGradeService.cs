using ExamService.Models;
using System.Collections.Generic;

namespace ExamService.Services
{
    public interface IGradeService
    {
        List<GradeDetails> GetAll();
        int Create(GradeDetails gradeDetails);
    }
}