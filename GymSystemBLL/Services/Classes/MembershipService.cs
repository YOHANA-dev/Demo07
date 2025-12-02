using AutoMapper;
using GymSystemBLL.Services.Interfaces;
using GymSystemBLL.ViewModels;
using GymSystemBLL.ViewModels.MembershipViewModels;
using GymSystemBLL.ViewModels.MemberViewModels;
using GymSystemDAL.Entities;
using GymSystemDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemBLL.Services.Classes
{
    public class MembershipService : IMembershipService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MembershipService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IEnumerable<MembershipViewModel> GetAllMemberships()
        {
            var memberships = _unitOfWork.MembershipRepository.GetAllMembershipsWithMembersAndPlans(m => m.Status.ToLower() == "active");
            var membershipViewModels = _mapper.Map<IEnumerable<MembershipViewModel>>(memberships);
            return membershipViewModels;
        }

        public bool CreateMembership(CreateMembershipViewModel createdMembership)
        {
            if (!IsMemberExists(createdMembership.MemberId) || !IsPlanExists(createdMembership.PlanId) || HasActiveMembership(createdMembership.MemberId))
                return false;

            var membershipRepo = _unitOfWork.GetRepository<Membership>();
            var MembershipToCreate = _mapper.Map<Membership>(createdMembership);

            var plan = _unitOfWork.GetRepository<Plan>().GetById(createdMembership.PlanId);
            MembershipToCreate.EndDate = DateTime.UtcNow.AddDays(plan.DurationDays);

            membershipRepo.Add(MembershipToCreate);
            return _unitOfWork.SaveChanges() > 0;
        }

        public IEnumerable<PlanForSelectListViewModel> GetPlansForDropdown()
        {
            var plans = _unitOfWork.GetRepository<Plan>().GetAll(p => p.IsActive);
            var planViewModels = _mapper.Map<IEnumerable<PlanForSelectListViewModel>>(plans);
            return planViewModels;
        }

        public IEnumerable<MemberForSelectListViewModel> GetMembersForDropdown()
        {
            var members = _unitOfWork.GetRepository<Member>().GetAll();
            var memberViewModels = _mapper.Map<IEnumerable<MemberForSelectListViewModel>>(members);
            return memberViewModels;
        }

        public bool DeleteMembership(int memberId)
        {
            var membershipRepo = _unitOfWork.MembershipRepository;
            var membership = membershipRepo.GetFirstOrDefault(m=>m.MemberId == memberId && m.Status.ToLower() == "active");
            if (membership is null)
                return false;
            membershipRepo.Delete(membership);
            return _unitOfWork.SaveChanges() > 0;
        }

        #region Helper Methods

        private bool IsMemberExists(int memberId)
            => _unitOfWork.GetRepository<Member>().GetById(memberId) is not null;

        private bool IsPlanExists(int planId)
            => _unitOfWork.GetRepository<Plan>().GetById(planId) is not null;

        private bool HasActiveMembership(int memberId)
            => _unitOfWork.MembershipRepository.GetAllMembershipsWithMembersAndPlans(m => m.MemberId == memberId && m.Status.ToLower() == "active").Any();

        #endregion
    }
}
