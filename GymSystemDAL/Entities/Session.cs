using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemDAL.Entities
{
    public class Session : BaseEntity
    {
        public string Description { get; set; } = null!;
        public int Capacity { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        #region 1:M RS Betweem Session Category

        // FK
        public int CategoryId { get; set; }
        public Category SessionCategory { get; set; } // ONE

        #endregion

        #region 1:M RS Betweem Session Trainer

        // FK
        public int TrainerId { get; set; }
        public Trainer SessionTrainer { get; set; }

        #endregion

        #region M:M RS Between Member Session
        
        public ICollection<MemberSession> SessionMembers { get; set; }
        
        #endregion
    }
}
