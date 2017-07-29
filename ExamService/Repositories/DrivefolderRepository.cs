using ExamService.Data;
using System.Collections.Generic;
using System.Linq;
using System;

namespace ExamService.Repositories
{
    public class DriveFolderRepository : IDriveFolderRepository
    {
        private IcasEntities icasEntities = new IcasEntities();

        public DriveFolder GetByDriveId(string driveId)
        {
            return icasEntities.DriveFolders.Where(df => df.DriveId == driveId).FirstOrDefault();
        }

        public List<DriveFolder> GetAll()
        {
            return icasEntities.DriveFolders.ToList();
        }

        public DriveFolder GetById(int id)
        {
            return icasEntities.DriveFolders.Where(df => df.Id == id).FirstOrDefault();
        }
    }
}