using AutoMapper;
using ExamService.Data;
using ExamService.Models;
using ExamService.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExamService.Services
{
    public class IcasExamService : IIcasExamService
    {
        private IExamRepository ExamRepository { get; set; }

        public IcasExamService(IExamRepository examRepository)
        {
            ExamRepository = examRepository;
        }

        public List<ExamPaperDetails> GetAll()
        {
            var exams = ExamRepository.GetAll();
            IMapper mapper = GetMapperForDto();

            return mapper.Map<List<ExamPaper>, List<ExamPaperDetails>>(exams.ToList());
        }

        public List<ExamPaperDetails> GetByGrade(int gradeId)
        {
            var exams = ExamRepository.GetAll()
                .Where(ex => ex.GradeId == gradeId);
            IMapper mapper = GetMapperForDto();

            return mapper.Map<List<ExamPaper>, List<ExamPaperDetails>>(exams.ToList());
        }

        public List<ExamPaperDetails> GetBySubject(int subjectId)
        {
            var exams = ExamRepository.GetAll()
                .Where(ex => ex.SubjectId == subjectId);
            IMapper mapper = GetMapperForDto();

            return mapper.Map<List<ExamPaper>, List<ExamPaperDetails>>(exams.ToList());
        }

        public int Create(ExamPaperDetails examPaperDetails)
        {
            IMapper mapper = GetMapperForEntity();
            var examPaper = mapper.Map<ExamPaper>(examPaperDetails);

            return ExamRepository.Insert(examPaper);
        }

        public void Update(ExamPaperDetails examPaperDetails)
        {
            throw new NotImplementedException();
        }

        public void Delete(int examPaperId)
        {
            throw new NotImplementedException();
        }

        public List<ExamPaperDetails> GetByIds(List<int> ids)
        {
            var exams = ExamRepository.GetAll()
                .Where(ex => ids.Contains(ex.Id));
            IMapper mapper = GetMapperForDto();

            return mapper.Map<List<ExamPaper>, List<ExamPaperDetails>>(exams.ToList());
        }

        private static IMapper GetMapperForDto()
        {
            // Todo: Create a common extension method. Refer sherpa app)
            var mappingConfig = new MapperConfiguration(cf =>
            {
                cf.CreateMap<ExamPaper, ExamPaperDetails>();
            });

            IMapper mapper = mappingConfig.CreateMapper();
            return mapper;
        }

        private static IMapper GetMapperForEntity()
        {
            // Todo: Create a common extension method. Refer sherpa app)
            var mappingConfig = new MapperConfiguration(cf =>
            {
                cf.CreateMap<ExamPaperDetails, ExamPaper>();
            });

            IMapper mapper = mappingConfig.CreateMapper();
            return mapper;
        }
    }
}