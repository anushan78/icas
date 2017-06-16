using AutoMapper;
using ExamService.Data;
using ExamService.Models;
using ExamService.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace ExamService.Services
{
    public class GradeService : IGradeService
    {
        private IGradeRepository GradeRepository { get; set; }

        public GradeService(IGradeRepository gradeRepository)
        {
            GradeRepository = gradeRepository;
        }

        public List<GradeDetails> GetAll()
        {
            var grades = GradeRepository.GetAll();
            IMapper mapper = GetMapperForDto();

            return mapper.Map<List<Grade>, List<GradeDetails>>(grades.ToList());
        }

        public int Create(GradeDetails gradeDetails)
        {
            IMapper mapper = GetMapperForEntity();
            var grade = mapper.Map<Grade>(gradeDetails);

            return GradeRepository.Insert(grade);
        }

        private static IMapper GetMapperForDto()
        {
            // Todo: Create a common extension method. Refer sherpa app)
            var mappingConfig = new MapperConfiguration(cf =>
            {
                cf.CreateMap<Grade, GradeDetails>();
            });

            IMapper mapper = mappingConfig.CreateMapper();
            return mapper;
        }

        private static IMapper GetMapperForEntity()
        {
            // Todo: Create a common extension method. Refer sherpa app)
            var mappingConfig = new MapperConfiguration(cf =>
            {
                cf.CreateMap<GradeDetails, Grade>();
            });

            IMapper mapper = mappingConfig.CreateMapper();
            return mapper;
        }
    }
}