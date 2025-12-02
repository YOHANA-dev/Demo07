using GymSystemBLL.ViewModels.MembershipViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemBLL.Services.Interfaces
{
    public interface IMembershipService
    {
        IEnumerable<MembershipViewModel> GetAllMemberships();

        IEnumerable<PlanForSelectListViewModel> GetPlansForDropdown();
        IEnumerable<MemberForSelectListViewModel> GetMembersForDropdown();
        bool CreateMembership(CreateMembershipViewModel createdMembership);

        bool DeleteMembership(int memberId);
    }
}
