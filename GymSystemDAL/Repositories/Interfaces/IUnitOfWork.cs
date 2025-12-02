using GymSystemDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemDAL.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        // Call IUnitOfWork for Asking Specific Repository
        // Method to Return Repository
        // Method to Save Changes

        IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new();
        int SaveChanges();

        public ISessionRepository SessionRepository { get; }
        public IMembershipRepository MembershipRepository { get; }
        public IBookingRepository BookingRepository { get; }
    }
}
