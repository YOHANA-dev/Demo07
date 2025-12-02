using GymSystemDAL.Data.Contexts;
using GymSystemDAL.Entities;
using GymSystemDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemDAL.Repositories.Classes
{
    public class PlanRepository : IPlanRepository
    {
        private readonly GymSystemDbContext _dbContext;

        public PlanRepository(GymSystemDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IEnumerable<Plan> GetAll()
        {
            return _dbContext.Plans.ToList();
        }

        public Plan? GetById(int id)
        {
            return _dbContext.Plans.Find(id);
        }

        public int Update(Plan plan)
        {
            _dbContext.Plans.Update(plan);
            return _dbContext.SaveChanges();
        }
    }
}
