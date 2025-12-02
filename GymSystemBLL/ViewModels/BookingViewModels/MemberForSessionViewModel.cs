using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemBLL.ViewModels.BookingViewModels
{
    public class MemberForSessionViewModel
    {
        public string MemberName { get; set; }
        public string BookingDate { get; set; }
        public int MemberId { get; set; }
        public bool isAttended { get; set; }
    }
}
