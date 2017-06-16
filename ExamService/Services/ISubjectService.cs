using ExamService.Models;
using System.Collections.Generic;

namespace ExamService.Services
{
    public interface ISubjectService
    {
        List<SubjectDetails> GetAll();
        int Create(SubjectDetails subjectDetails);
    }
}