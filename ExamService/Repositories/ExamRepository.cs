using ExamService.Data;
using System.Collections.Generic;
using System.Linq;

namespace ExamService.Repositories
{
    public class ExamRepository : IExamRepository
    {
        private IcasEntities icasEntities = new IcasEntities();

        public void Delete(ExamPaper examPaper)
        {
            icasEntities.ExamPapers.Remove(examPaper);
            icasEntities.SaveChanges();
        }

        public List<ExamPaper> GetAll()
        {
            return icasEntities.ExamPapers.ToList();
        }

        public ExamPaper GetById(int Id)
        {
            return icasEntities.ExamPapers.Where(ep => ep.Id == Id).FirstOrDefault();
        }

        public int Insert(ExamPaper examPaper)
        {
            icasEntities.ExamPapers.Add(examPaper);
            icasEntities.SaveChanges();

            return examPaper.Id;
        }

        public void Update(ExamPaper examPaper)
        {
            icasEntities.ExamPapers.Attach(examPaper);
            icasEntities.SaveChanges();
        }
    }
}