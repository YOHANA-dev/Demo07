using GymSystemBLL.Services.AttachmentService;
using GymSystemBLL.Services.Interfaces;
using GymSystemBLL.ViewModels;
using GymSystemBLL.ViewModels.MemberViewModels;
using GymSystemDAL.Entities;
using GymSystemDAL.Repositories.Classes;
using GymSystemDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemBLL.Services.Classes
{
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAttachmentService _attachmentService;

        // Connection DB

        public MemberService(IUnitOfWork unitOfWork, IAttachmentService attachmentService)
        {
            _unitOfWork = unitOfWork;
            _attachmentService = attachmentService;
        }
        // Don't Forget to Register IUnitOfWork in Program.cs

        public bool CreateMembers(CreateMemberViewModel createdMember)
        {
            // Check if Email and Email are unique
            try
            {
                if (IsEmailExists(createdMember.Email) || IsPhoneExists(createdMember.Phone)) return false;

                var PhotoName = _attachmentService.Upload("members", createdMember.PhotoFile);
                if (string.IsNullOrEmpty(PhotoName)) return false;

                var member = new Member()
                {
                    Name = createdMember.Name,
                    Email = createdMember.Email,
                    Phone = createdMember.Phone,
                    DateOfBirth = createdMember.DateOfBirth,
                    Gender = createdMember.Gender,
                    Address = new Address()
                    {
                        BuildingNumber = createdMember.BuildingNumber,
                        Street = createdMember.Street,
                        City = createdMember.City
                    },
                    HealthRecord = new HealthRecord()
                    {
                        Weight = createdMember.HealthViewModel.Weight,
                        Height = createdMember.HealthViewModel.Height,
                        BloodType = createdMember.HealthViewModel.BloodType,
                        Note = createdMember.HealthViewModel.Note
                    }
                };

                member.Photo = PhotoName;

                _unitOfWork.GetRepository<Member>().Add(member);

                var isCreated = _unitOfWork.SaveChanges() > 0;
                if (!isCreated)
                {
                    _attachmentService.Delete("members", PhotoName);
                    return false;
                }
                else
                {
                    return isCreated;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<MemberViewModel> GetAllMembers()
        {
            #region First Way Of Mapping

            //    //var members = _memberRepository.GetAll() ?? [];
            //    var Members = _memberRepository.GetAll() ?? [];
            //    if (Members is null || Members.Any()) return [];

            //    var MemberViewModels = new List<MemberViewModel>();
            //    foreach (var Member in Members)
            //    {
            //        var memberViewModel = new MemberViewModel()
            //        {
            //            Id = Member.Id,
            //            Photo = Member.Photo,
            //            Name = Member.Name,
            //            Email = Member.Email,
            //            Phone = Member.Phone,
            //            Gender = Member.Gender.ToString();
            //        };
            //        MemberViewModels.Add(memberViewModel);
            //    }
            //    return MemberViewModels;

            #endregion

            var Members = _unitOfWork.GetRepository<Member>().GetAll();
            if (Members is null || !Members.Any()) return [];

            var MemberViewModels = Members.Select(Member => new MemberViewModel()
            {
                Id = Member.Id,
                Photo = Member.Photo,
                Name = Member.Name,
                Email = Member.Email,
                Phone = Member.Phone,
                Gender = Member.Gender.ToString()
            });
            return MemberViewModels;
        }

        public MemberViewModel? GetMemberDetails(int memberId)
        {
            // IPlanRepository
            // Inject for PlanRepo and MembershipRepo
            var Member = _unitOfWork.GetRepository<Member>().GetById(memberId);
            if (Member is null) return null;

            var viewModel = new MemberViewModel()
            {
                Id = Member.Id,
                Photo = Member.Photo,
                Name = Member.Name,
                Email = Member.Email,
                Phone = Member.Phone,
                Gender = Member.Gender.ToString(),
                DateOfBirth = Member.DateOfBirth.ToShortDateString(),
                Address = $"{Member.Address.BuildingNumber}, {Member.Address.Street}, {Member.Address.City}",
            };

            var ActiveMembership = _unitOfWork.GetRepository<Membership>()
                .GetAll(m => m.MemberId == memberId && m.Status == "Active").FirstOrDefault();

            if (ActiveMembership is not null) // StartDate , EndDate
            {
                viewModel.MembershipStartDate = ActiveMembership.CreatedAt.ToShortDateString();
                viewModel.MembershipEndDate = ActiveMembership.EndDate.ToShortDateString();

                // Plans
                var Plan = _unitOfWork.GetRepository<Plan>().GetById(ActiveMembership.PlanId);
                viewModel.PlanName = Plan?.Name;
            }
            return viewModel;
        }

        public HealthViewModel? GetMemberHealthRecordDetails(int memberId)
        {
            var MemberHealthRecord = _unitOfWork.GetRepository<HealthRecord>().GetById(memberId);
            if (MemberHealthRecord is null) return null;

            return new HealthViewModel()
            {
                Weight = MemberHealthRecord.Weight,
                Height = MemberHealthRecord.Height,
                BloodType = MemberHealthRecord.BloodType,
                Note = MemberHealthRecord.Note
            };
        }

        public MemberToUpdateViewModel? GetMemberToUpdate(int memberId)
        {
            var Member = _unitOfWork.GetRepository<Member>().GetById(memberId);
            if (Member is null) return null;

            return new MemberToUpdateViewModel()
            {
                Name = Member.Name,
                Photo = Member.Photo,
                Email = Member.Email,
                Phone = Member.Phone,
                BuildingNumber = Member.Address.BuildingNumber,
                Street = Member.Address.Street,
                City = Member.Address.City
            };
        }

        public bool UpdateMemberDetails(int id, MemberToUpdateViewModel updatedMember)
        {
            try
            {
                //if (IsEmailExists(updatedMember.Email) || IsPhoneExists(updatedMember.Phone)) return false;
                var EmailExists = _unitOfWork.GetRepository<Member>()
                    .GetAll(X => X.Email == updatedMember.Email && X.Id != id);

                var PhoneExists = _unitOfWork.GetRepository<Member>()
                    .GetAll(X => X.Phone == updatedMember.Phone && X.Id != id);

                if (EmailExists.Any() || PhoneExists.Any()) return false;

                var Member = _unitOfWork.GetRepository<Member>().GetById(id);
                if (Member is null) return false;

                Member.Email = updatedMember.Email;
                Member.Phone = updatedMember.Phone;
                Member.Address.BuildingNumber = updatedMember.BuildingNumber;
                Member.Address.Street = updatedMember.Street;
                Member.Address.City = updatedMember.City;
                Member.UpdatedAt = DateTime.Now;
                _unitOfWork.GetRepository<Member>().Update(Member);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool RemoveMember(int memberId)
        {
            var Member = _unitOfWork.GetRepository<Member>().GetById(memberId);
            if (Member is null) return false;

            // Check if member has active sessions or not
            //var HasActiveMemberSessions = _unitOfWork.GetRepository<MemberSession>()
            //    .GetAll(ms => ms.MemberId == memberId && ms.Session.StartDate > DateTime.Now).Any();

            // Get All Sessions Ids
            var SessionIDs = _unitOfWork.GetRepository<MemberSession>()
                .GetAll(X => X.MemberId == memberId).Select(X => X.SessionId);

            var HasActiveMemberSessions = _unitOfWork.GetRepository<Session>()
                .GetAll(X => SessionIDs.Contains(X.Id) && X.StartDate > DateTime.Now).Any();

            if (HasActiveMemberSessions) return false;

            // Remove
            // Handle to Cascade Action in Code
            var Membership = _unitOfWork.GetRepository<Membership>().GetAll(m => m.MemberId == memberId);
            try
            {
                if (Membership.Any())
                {
                    foreach (var membership in Membership)
                    {
                        _unitOfWork.GetRepository<Membership>().Delete(membership);
                    }
                }
                _unitOfWork.GetRepository<Member>().Delete(Member);

                var isDeleted = _unitOfWork.SaveChanges() > 0;

                if (isDeleted)
                {
                    _attachmentService.Delete("members", Member.Photo);
                }
                return isDeleted;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #region Helper Methods

        private bool IsEmailExists(string email)
        {
            return _unitOfWork.GetRepository<Member>().GetAll(m => m.Email == email).Any();
        }

        private bool IsPhoneExists(string phone)
        {
            return _unitOfWork.GetRepository<Member>().GetAll(m => m.Phone == phone).Any();
        }

        #endregion
    }
}
