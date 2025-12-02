using GymSystemDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemDAL.Repositories.Interfaces
{
    public interface ISessionRepository : IGenericRepository<Session>
    {
        IEnumerable<Session> GetAllSessionsWithTrainerAndCategory();
        int GetCountOfBookedSlots(int sessionId);
        Session? GetSessionWithTrainerAndCategory(int sessionId);
    }
}
