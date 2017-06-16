using ExamService.Data;
using System.Collections.Generic;
using System.Linq;

namespace ExamService.Repositories
{
    public class SubjectRepository : ISubjectRepository
    {
        private IcasEntities icasEntities = new IcasEntities();

        public List<Subject> GetAll()
        {
            return icasEntities.Subjects.ToList();
        }

        public Subject GetById(int Id)
        {
            return icasEntities.Subjects.Where(su => su.Id == Id).FirstOrDefault();
        }

        public int Insert(Subject subject)
        {
            icasEntities.Subjects.Add(subject);
            icasEntities.SaveChanges();

            return subject.Id;
        }
    }
}