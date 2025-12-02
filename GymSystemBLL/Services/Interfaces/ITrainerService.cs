using GymSystemBLL.ViewModels.MemberViewModels;
using GymSystemBLL.ViewModels.TrainerViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemBLL.Services.Interfaces
{
    public interface ITrainerService
    {
        IEnumerable<TrainerViewModel> GetAllTrainers();

        bool CreateTrainers(CreateTrainerViewModel createdTrainer);

        TrainerViewModel? GetTrainerDetails(int trainerId);

        TrainerToUpdateViewModel? GetTrainerToUpdate(int trainerId);

        bool UpdateTrainerDetails(int id, TrainerToUpdateViewModel updatedTrainer);

        bool RemoveTrainer(int trainerId);
    }
}
