using GymSystemBLL.Services.Interfaces;
using GymSystemBLL.ViewModels.TrainerViewModels;
using GymSystemDAL.Entities;
using GymSystemDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemBLL.Services.Classes
{
    public class TrainerService : ITrainerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TrainerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public bool CreateTrainers(CreateTrainerViewModel createdTrainer)
        {
            try
            {
                if (IsEmailExists(createdTrainer.Email) || IsPhoneExists(createdTrainer.Phone)) return false;

                var trainer = new Trainer()
                {
                    Name = createdTrainer.Name,
                    Email = createdTrainer.Email,
                    Phone = createdTrainer.Phone,
                    DateOfBirth = createdTrainer.DateOfBirth,
                    Specialties = createdTrainer.Specialties,
                    Address = new Address()
                    {
                        BuildingNumber = createdTrainer.BuildingNumber,
                        Street = createdTrainer.Street,
                        City = createdTrainer.City
                    }
                };
                _unitOfWork.GetRepository<Trainer>().Add(trainer);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<TrainerViewModel> GetAllTrainers()
        {
            var trainers = _unitOfWork.GetRepository<Trainer>().GetAll();
            if (trainers is null || !trainers.Any()) return [];

            var trainerViewModels = trainers.Select(t => new TrainerViewModel()
            {
                Id = t.Id,
                Name = t.Name,
                Email = t.Email,
                Phone = t.Phone,
                Specialties = t.Specialties.ToString()
            });
            return trainerViewModels;
        }

        public TrainerViewModel? GetTrainerDetails(int trainerId)
        {
            var trainer = _unitOfWork.GetRepository<Trainer>().GetById(trainerId);
            if (trainer is null) return null;

            var trainerViewModel = new TrainerViewModel()
            {
                Id = trainer.Id,
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.Phone,
                Specialties = trainer.Specialties.ToString(),
                DateOfBirth = trainer.DateOfBirth.ToShortDateString(),
                Address = trainer.Address is not null ? $"{trainer.Address.BuildingNumber}, {trainer.Address.Street}, {trainer.Address.City}" : null,
                JobTitle = $"{trainer.Specialties} Trainer"
            };
            return trainerViewModel;
        }

        public TrainerToUpdateViewModel? GetTrainerToUpdate(int trainerId)
        {
            var trainer = _unitOfWork.GetRepository<Trainer>().GetById(trainerId);
            if (trainer is null) return null;

            return new TrainerToUpdateViewModel()
            {
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.Phone,
                Specialties = trainer.Specialties,
                DateOfBirth = trainer.DateOfBirth,
                BuildingNumber = trainer.Address.BuildingNumber,
                Street = trainer.Address.Street,
                City = trainer.Address.City
            };
        }

        public bool RemoveTrainer(int trainerId)
        {
            try
            {
                var trainer = _unitOfWork.GetRepository<Trainer>().GetById(trainerId);
                if (trainer is null) return false;

                var hasFutureSessions = trainer.TrainerSessions?.Any(s => s.CreatedAt > DateTime.Now) == true;
                if (hasFutureSessions) return false;

                _unitOfWork.GetRepository<Trainer>().Delete(trainer);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UpdateTrainerDetails(int id, TrainerToUpdateViewModel updatedTrainer)
        {
            try
            {
                var EmailExists = _unitOfWork.GetRepository<Trainer>()
                    .GetAll(X => X.Email == updatedTrainer.Email && X.Id != id);

                var PhoneExists = _unitOfWork.GetRepository<Trainer>()
                    .GetAll(X => X.Phone == updatedTrainer.Phone && X.Id != id);

                if (EmailExists.Any() || PhoneExists.Any()) return false;

                var trainer = _unitOfWork.GetRepository<Trainer>().GetById(id);
                if (trainer is null) return false;

                trainer.Name = updatedTrainer.Name;
                trainer.Email = updatedTrainer.Email;
                trainer.Phone = updatedTrainer.Phone;
                trainer.Specialties = updatedTrainer.Specialties;
                trainer.DateOfBirth = updatedTrainer.DateOfBirth;
                trainer.Address.BuildingNumber = updatedTrainer.BuildingNumber;
                trainer.Address.Street = updatedTrainer.Street;
                trainer.Address.City = updatedTrainer.City;
                _unitOfWork.GetRepository<Trainer>().Update(trainer);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #region Helper Methods

        private bool IsEmailExists(string email)
        {
            return _unitOfWork.GetRepository<Trainer>().GetAll(m => m.Email == email).Any();
        }

        private bool IsPhoneExists(string phone)
        {
            return _unitOfWork.GetRepository<Trainer>().GetAll(m => m.Phone == phone).Any();
        }

        #endregion
    }
}
