using GymSystemBLL.ViewModels;
using GymSystemBLL.ViewModels.MemberViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemBLL.Services.Interfaces
{
    public interface IMemberService
    {
        IEnumerable<MemberViewModel> GetAllMembers();

        bool CreateMembers(CreateMemberViewModel createdMember);

        MemberViewModel? GetMemberDetails(int memberId);

        // Get HealthRecord
        HealthViewModel? GetMemberHealthRecordDetails(int memberId);

        // Get MemberId to update View
        MemberToUpdateViewModel? GetMemberToUpdate(int memberId);

        // Apply Update
        bool UpdateMemberDetails(int id, MemberToUpdateViewModel updatedMember);
    
        // Remove
        bool RemoveMember(int memberId);
    }
}
