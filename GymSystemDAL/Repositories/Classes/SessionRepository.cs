using GymSystemDAL.Data.Contexts;
using GymSystemDAL.Entities;
using GymSystemDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemDAL.Repositories.Classes
{
    public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {
        private readonly GymSystemDbContext _dbContext;

        // Chain Parent Constructor
        public SessionRepository(GymSystemDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Session> GetAllSessionsWithTrainerAndCategory()
        {
            return _dbContext.Sessions.Include(s => s.SessionTrainer)
                                      .Include(s => s.SessionCategory)
                                      .ToList();
        }

        public int GetCountOfBookedSlots(int sessionId)
        {
            return _dbContext.MemberSessions.Count(s => s.SessionId == sessionId);
        }

        public Session? GetSessionWithTrainerAndCategory(int sessionId)
        {
            return _dbContext.Sessions
                             .Include(s => s.SessionTrainer)
                             .Include(s => s.SessionCategory)
                             .FirstOrDefault(s => s.Id == sessionId);
        }
    }
}
