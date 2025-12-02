using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemDAL.Entities
{
    public class Membership : BaseEntity
    {
        public int MemberId { get; set; } // FK to Member
        public Member Member { get; set; }
        public int PlanId { get; set; } // FK to Plan
        public Plan Plan { get; set; }

        // StartDate => CreatedAt BaseEntity
        public DateTime EndDate { get; set; }

        public string Status
        {
            get
            {
                if(EndDate <= DateTime.Now)
                {
                    return "Expired";
                }
                else
                {
                    return "Active";
                }
            }
        }
    }
}
