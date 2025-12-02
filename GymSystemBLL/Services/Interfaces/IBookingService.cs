using GymSystemBLL.ViewModels.BookingViewModels;
using GymSystemBLL.ViewModels.MembershipViewModels;
using GymSystemBLL.ViewModels.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemBLL.Services.Interfaces
{
    public interface IBookingService
    {
        IEnumerable<SessionViewModel> GetAllSessionsWithTrainerAndCategories();

        IEnumerable<MemberForSessionViewModel> GetAllMembersForSession(int id);

        MemberForSessionViewModel? MarkMemberAttendance(int memberId, int sessionId);

        bool CreateBooking(CreateBookingViewModel model);

        IEnumerable<MemberForSelectListViewModel> GetMembersForDropDown(int id);

        bool DeleteBooking(int memberId, int sessionId);
    }
}
