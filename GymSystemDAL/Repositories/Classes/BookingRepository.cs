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
    public class BookingRepository : GenericRepository<MemberSession>, IBookingRepository
    {
        private readonly GymSystemDbContext _dbContext;

        public BookingRepository(GymSystemDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<MemberSession> GetSessionById(int sessionId)
        {
            return _dbContext.MemberSessions.Where(ms => ms.SessionId == sessionId)
                                            .Include(ms => ms.Member).ToList();
        }
    }
}
