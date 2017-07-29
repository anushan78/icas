using ExamService.Models;
using System.Collections.Generic;

namespace ExamService.Services
{
    public interface IDriveFolderService
    {
        List<DriveFolderDetails> GetAll();
    }
}