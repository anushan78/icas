using ExamService.Data;
using System.Collections.Generic;

namespace ExamService.Repositories
{
    public interface IDriveFolderRepository
    {
        List<DriveFolder> GetAll();
        DriveFolder GetById(int id);
        DriveFolder GetByDriveId(string driveId);
    }
}
