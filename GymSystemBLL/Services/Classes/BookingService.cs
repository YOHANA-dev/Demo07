using AutoMapper;
using GymSystemBLL.Services.Interfaces;
using GymSystemBLL.ViewModels.BookingViewModels;
using GymSystemBLL.ViewModels.MembershipViewModels;
using GymSystemBLL.ViewModels.SessionViewModels;
using GymSystemDAL.Entities;
using GymSystemDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemBLL.Services.Classes
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BookingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public bool CreateBooking(CreateBookingViewModel model)
        {
            var bookingRepository = _unitOfWork.BookingRepository;
            var sessionRepository = _unitOfWork.SessionRepository;
            var membershipRepository = _unitOfWork.MembershipRepository;
            var session = sessionRepository.GetById(model.SessionId);
            if (session == null || session.StartDate <= DateTime.UtcNow)
            {
                return false;
            }
            var activeMembership = membershipRepository.GetFirstOrDefault(m => m.Status.ToLower() == "Active".ToLower() && m.MemberId == model.MemberId);
            if (activeMembership == null)
            {
                return false;
            }
            var bookedSlotsCount = sessionRepository.GetCountOfBookedSlots(model.SessionId);
            if (bookedSlotsCount >= session.Capacity)
            {
                return false;
            }
            var existingBooking = bookingRepository.GetAll(b => b.SessionId == model.SessionId && b.MemberId == model.MemberId).FirstOrDefault();
            if (existingBooking != null)
            {
                return false;
            }
            var bookingEntity = _mapper.Map<MemberSession>(model);
            bookingEntity.CreatedAt = DateTime.Now;
            bookingEntity.IsAttended = false;
            bookingRepository.Add(bookingEntity);
            return _unitOfWork.SaveChanges() > 0;
        }

        public IEnumerable<MemberForSessionViewModel> GetAllMembersForSession(int id)
        {
            var bookingRepository = _unitOfWork.BookingRepository;
            var bookings = bookingRepository.GetSessionById(id);
            var memberViewModels = _mapper.Map<IEnumerable<MemberForSessionViewModel>>(bookings);
            return memberViewModels;
        }

        public IEnumerable<SessionViewModel> GetAllSessionsWithTrainerAndCategories()
        {
            var sessionRepository = _unitOfWork.SessionRepository;
            var sessions = sessionRepository.GetAllSessionsWithTrainerAndCategory();
            var sessionViewModels = _mapper.Map<IEnumerable<SessionViewModel>>(sessions);
            foreach (var session in sessionViewModels)
            {
                session.AvailableSlots = session.Capacity - sessionRepository.GetCountOfBookedSlots(session.Id);
            }
            return sessionViewModels;
        }

        public MemberForSessionViewModel? MarkMemberAttendance(int memberId, int sessionId)
        {
            var bookingRepository = _unitOfWork.BookingRepository;
            var booking = bookingRepository.GetSessionById(sessionId)
                                           .FirstOrDefault(ms => ms.MemberId == memberId);
            if (booking != null)
            {
                booking.IsAttended = true;
                bookingRepository.Update(booking);
                _unitOfWork.SaveChanges();
                var memberViewModel = _mapper.Map<MemberForSessionViewModel>(booking);
                return memberViewModel;
            }
            return null;
        }

        #region Helper Methods

        public IEnumerable<MemberForSelectListViewModel> GetMembersForDropDown(int id)
        {
            var bookingRepository = _unitOfWork.BookingRepository;
            var bookings = bookingRepository.GetAll(b => b.Id == id).Select(m => m.MemberId).ToList();
            var membersAvailableToBook = _unitOfWork.GetRepository<Member>().GetAll(m => !bookings.Contains(m.Id));
            var memberViewModels = _mapper.Map<IEnumerable<MemberForSelectListViewModel>>(membersAvailableToBook);
            return memberViewModels;
        }

        public bool DeleteBooking(int memberId, int sessionId)
        {
            var bookingRepository = _unitOfWork.BookingRepository;
            var booking = bookingRepository.GetAll(b => b.MemberId == memberId && b.SessionId == sessionId).FirstOrDefault();
            if (booking != null)
            {
                bookingRepository.Delete(booking);
                return _unitOfWork.SaveChanges() > 0;
            }
            return false;
        }

        #endregion
    }
}
