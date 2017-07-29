using AutoMapper;
using ExamService.Data;
using ExamService.Models;
using ExamService.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace ExamService.Services
{
    public class DriveFolderService : IDriveFolderService
    {
        private IDriveFolderRepository DriveFolderRepository { get; set; }

        public DriveFolderService(IDriveFolderRepository driveFolderRepository)
        {
            DriveFolderRepository = driveFolderRepository;
        }

        public List<DriveFolderDetails> GetAll()
        {
            var folders = DriveFolderRepository.GetAll();
            IMapper mapper = GetMapperForDto();

            return mapper.Map<List<DriveFolder>, List<DriveFolderDetails>>(folders.ToList());
        }

        private static IMapper GetMapperForDto()
        {
            // Todo: Create a common extension method. Refer sherpa app)
            var mappingConfig = new MapperConfiguration(cf =>
            {
                cf.CreateMap<DriveFolder, DriveFolderDetails>();
            });

            IMapper mapper = mappingConfig.CreateMapper();
            return mapper;
        }
    }
}