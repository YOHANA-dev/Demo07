using AutoMapper;
using GymSystemBLL.Services.Interfaces;
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
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SessionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public bool CreateSession(CreateSessionViewModel createdSession)
        {
            try
            {
                // Check If Trainer exists
                // Check If Category exists
                // Check If StartDate < EndDate
                if (!IsTrainerExists(createdSession.TrainerId)) return false;
                if (!IsCategoryExists(createdSession.CategoryId)) return false;
                if (!IsDateTimeValid(createdSession.StartDate, createdSession.EndDate)) return false;
                if (createdSession.Capacity < 0 || createdSession.Capacity > 25) return false;

                var SessionEntity = _mapper.Map<Session>(createdSession);
                _unitOfWork.SessionRepository.Add(SessionEntity);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<SessionViewModel> GetAllSessions()
        {
            var Sessions = _unitOfWork.SessionRepository.GetAllSessionsWithTrainerAndCategory();
            if (Sessions == null || !Sessions.Any()) return [];

            return Sessions.Select(s => new SessionViewModel
            {
                Id = s.Id,
                CategoryName = s.SessionCategory.CategoryName,
                Description = s.Description,
                TrainerName = s.SessionTrainer.Name,
                StartDate = s.StartDate,
                EndDate = s.EndDate,
                Capacity = s.Capacity,
                AvailableSlots = s.Capacity - _unitOfWork.SessionRepository.GetCountOfBookedSlots(s.Id)
            });
        }

        public SessionViewModel? GetSessionById(int sessionId)
        {
            var Session = _unitOfWork.SessionRepository.GetSessionWithTrainerAndCategory(sessionId);
            if (Session == null) return null;

            ///return new SessionViewModel
            ///{
            ///    Id = Session.Id,
            ///    CategoryName = Session.SessionCategory.CategoryName,
            ///    Description = Session.Description,
            ///    TrainerName = Session.SessionTrainer.Name,
            ///    StartDate = Session.StartDate,
            ///    EndDate = Session.EndDate,
            ///    Capacity = Session.Capacity,
            ///    AvailableSlots = Session.Capacity - _unitOfWork.SessionRepository.GetCountOfBookedSlots(Session.Id)
            ///};

            // Allow AutoMapper
            var MappedSession = _mapper.Map<SessionViewModel>(Session);
            MappedSession.AvailableSlots = Session.Capacity - _unitOfWork.SessionRepository.GetCountOfBookedSlots(Session.Id);
            return MappedSession;
        }

        public UpdateSessionViewModel? GetSessionToUpdate(int sessionId)
        {
            var Session = _unitOfWork.GetRepository<Session>().GetById(sessionId);

            if (!IsSessionAvailableForUpdate(Session)) return null;

            return _mapper.Map<UpdateSessionViewModel>(Session);
        }

        public bool UpdateSession(UpdateSessionViewModel updatedSession, int sessionId)
        {
            try
            {
                var Session = _unitOfWork.SessionRepository.GetById(sessionId);
                if (!IsSessionAvailableForUpdate(Session!)) return false;
                if (!IsTrainerExists(updatedSession.TrainerId)) return false;
                if (!IsDateTimeValid(updatedSession.StartDate, updatedSession.EndDate)) return false;

                _mapper.Map(updatedSession, Session);
                Session!.UpdatedAt = DateTime.Now;

                _unitOfWork.SessionRepository.Update(Session);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool RemoveSession(int sessionId)
        {
            try
            {
                var Session = _unitOfWork.SessionRepository.GetById(sessionId);
                if (!IsSessionAvailableForDelete(Session!)) return false;

                _unitOfWork.SessionRepository.Delete(Session!);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<TrainerSelectViewModel> GetTrainerForSessions()
        {
            var Trainers = _unitOfWork.GetRepository<Trainer>().GetAll();
            return _mapper.Map<IEnumerable<TrainerSelectViewModel>>(Trainers);
        }

        public IEnumerable<CategorySelectViewModel> GetCategoryForSessions()
        {
            var Categories = _unitOfWork.GetRepository<Category>().GetAll();
            return _mapper.Map<IEnumerable<CategorySelectViewModel>>(Categories);
        }

        #region HelperMethods

        private bool IsTrainerExists(int trainerId)
        {
            return _unitOfWork.GetRepository<Trainer>().GetById(trainerId) != null;
        }

        private bool IsCategoryExists(int categoryId)
        {
            return _unitOfWork.GetRepository<Category>().GetById(categoryId) != null;
        }

        private bool IsDateTimeValid(DateTime startDate, DateTime endDate)
        {
            return startDate < endDate;
        }

        private bool IsSessionAvailableForUpdate(Session session)
        {
            if (session == null) return false;

            // If Session Completed => cannot Update
            if (session.EndDate < DateTime.Now) return false;
            // If Session is Ongoing => cannot Update
            if (session.StartDate <= DateTime.Now && session.EndDate > DateTime.Now) return false;
            // If Session has Active Booking => cannot Update
            var ActiveBookings = _unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id);
            if (ActiveBookings > 0) return false;

            return true;
        }

        private bool IsSessionAvailableForDelete(Session session)
        {
            if (session == null) return false;

            // If Session Upcoming => cannot Delete
            if (session.StartDate > DateTime.Now) return false;
            // If Session OnGoing => cannot Delete
            if (session.StartDate <= DateTime.Now && session.EndDate >= DateTime.Now) return false;
            // If Session has Active Booking => cannot Delete
            var ActiveBookings = _unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id);
            if (ActiveBookings > 0) return false;

            return true;
        }

        #endregion
    }
}
