using AutoMapper;
using ExamService.Data;
using ExamService.Models;
using ExamService.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace ExamService.Services
{
    public class SubjectService : ISubjectService
    {
        private ISubjectRepository SubjectRepository { get; set; }

        public SubjectService(ISubjectRepository subjectRepository)
        {
            SubjectRepository = subjectRepository;
        }

        public List<SubjectDetails> GetAll()
        {
            var subjects = SubjectRepository.GetAll();
            IMapper mapper = GetMapperForDto();

            return mapper.Map<List<Subject>, List<SubjectDetails>>(subjects.ToList());
        }

        public int Create(SubjectDetails subjectDetails)
        {
            IMapper mapper = GetMapperForEntity();
            var subject = mapper.Map<Subject>(subjectDetails);

            return SubjectRepository.Insert(subject);
        }

        private static IMapper GetMapperForDto()
        {
            // Todo: Create a common extension method. Refer sherpa app)
            var mappingConfig = new MapperConfiguration(cf =>
            {
                cf.CreateMap<Subject, SubjectDetails>();
            });

            IMapper mapper = mappingConfig.CreateMapper();
            return mapper;
        }

        private static IMapper GetMapperForEntity()
        {
            // Todo: Create a common extension method. Refer sherpa app)
            var mappingConfig = new MapperConfiguration(cf =>
            {
                cf.CreateMap<SubjectDetails, Subject>();
            });

            IMapper mapper = mappingConfig.CreateMapper();
            return mapper;
        }
    }
}