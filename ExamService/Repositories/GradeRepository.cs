using ExamService.Data;
using System.Collections.Generic;
using System.Linq;

namespace ExamService.Repositories
{
    public class GradeRepository : IGradeRepository
    {
        private IcasEntities icasEntities = new IcasEntities();

        public List<Grade> GetAll()
        {
            return icasEntities.Grades.ToList();
        }

        public Grade GetById(int Id)
        {
            return icasEntities.Grades.Where(gr => gr.Id == Id).FirstOrDefault();
        }

        public int Insert(Grade grade)
        {
            icasEntities.Grades.Add(grade);
            icasEntities.SaveChanges();

            return grade.Id;
        }
    }
}