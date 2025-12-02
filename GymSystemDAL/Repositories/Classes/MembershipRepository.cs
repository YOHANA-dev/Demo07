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
    public class MembershipRepository : GenericRepository<Membership>, IMembershipRepository
    {
        private readonly GymSystemDbContext _dbContext;

        public MembershipRepository(GymSystemDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Membership> GetAllMembershipsWithMembersAndPlans(Func<Membership, bool>? filter = null)
        {
            var memberships = _dbContext.Memberships.Include(m => m.Member)
                                                    .Include(m => m.Plan)
                                                    .Where(filter ?? (_ => true));

            return memberships;
        }

        public Membership? GetFirstOrDefault(Func<Membership, bool>? filter = null)
        {
            var membership = _dbContext.Memberships.FirstOrDefault(filter ?? (_ => true));
            return membership;
        }
    }
}
