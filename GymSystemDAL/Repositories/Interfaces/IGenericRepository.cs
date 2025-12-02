using GymSystemDAL.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemDAL.Repositories.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity , new()
    {
        // GetById
        TEntity? GetById(int id);

        // GetAll
        IEnumerable<TEntity> GetAll(Func<TEntity ,bool> condition = null);

        // Add
        void Add(TEntity entity);

        // Update
        void Update(TEntity entity);

        // Delete
        void Delete(TEntity entity);
    }
}
