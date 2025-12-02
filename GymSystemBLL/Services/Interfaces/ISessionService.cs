using GymSystemBLL.ViewModels.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemBLL.Services.Interfaces
{
    public interface ISessionService
    {
        IEnumerable<SessionViewModel> GetAllSessions();
        SessionViewModel? GetSessionById(int sessionId);
        bool CreateSession(CreateSessionViewModel createdSession);
        UpdateSessionViewModel? GetSessionToUpdate(int sessionId);
        bool UpdateSession(UpdateSessionViewModel updatedSession, int sessionId);
        bool RemoveSession(int sessionId);

        IEnumerable<TrainerSelectViewModel> GetTrainerForSessions();
        IEnumerable<CategorySelectViewModel> GetCategoryForSessions();
    }
}
