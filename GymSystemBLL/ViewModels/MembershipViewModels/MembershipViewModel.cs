using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemBLL.ViewModels.MembershipViewModels
{
    public class MembershipViewModel
    {
        public int MemberId { get; set; }
        public string MemberName { get; set; } = null!;
        public int PlanId { get; set; }
        public string PlanName { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }


        public string StartDateDisplay => $"{StartDate: dd/MM/yyyy}";
        public string EndDateDisplay => $"{EndDate: dd/MM/yyyy}";
    }
}
