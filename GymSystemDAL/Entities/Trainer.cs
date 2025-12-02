using GymSystemDAL.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemDAL.Entities
{
    public class Trainer : GymUser
    {
        // CreatedAt Column in BaseEntity
        // Will be used as HireDate for Trainer => Configurations

        public Specialties Specialties { get; set; }

        #region 1:M RS Betweem Session Trainer

        public ICollection<Session>? TrainerSessions { get; set; }

        #endregion
    }
}
