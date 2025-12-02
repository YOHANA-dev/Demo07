using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemDAL.Entities
{
    public class Member : GymUser
    {
        // CreatedAt Column in BaseEntity
        // Will be used as JoinDate for Member => Configurations

        public string Photo { get; set; } = null!;

        #region 1:1 RS Between Member HealthRecord

        // Nav Property
        public HealthRecord HealthRecord { get; set; } // ONE

        #endregion

        #region M:M RS Between Member Plan

        public ICollection<Membership> Memberships { get; set; }

        #endregion

        #region M:M RS Between Member Session

        public ICollection<MemberSession> MemberSessions { get; set; }

        #endregion
    }
}
