using GymSystemBLL.Services.Interfaces;
using GymSystemBLL.ViewModels.PlanViewModels;
using GymSystemDAL.Entities;
using GymSystemDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemBLL.Services.Classes
{
    public class PlanService : IPlanService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PlanService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<PlanViewModel> GetAllPlans()
        {
            var Plans = _unitOfWork.GetRepository<Plan>().GetAll();

            if (Plans is null || !Plans.Any()) return [];

            var PlanViewModels = Plans.Select(Plan => new PlanViewModel()
            {
                Id = Plan.Id,
                Name = Plan.Name,
                Description = Plan.Description,
                DurationDays = Plan.DurationDays,
                Price = Plan.Price,
                IsActive = Plan.IsActive
            });
            return PlanViewModels;
        }

        public PlanViewModel? GetPlanById(int id)
        {
            var Plan = _unitOfWork.GetRepository<Plan>().GetById(id);

            if (Plan is null) return null;
            return new PlanViewModel()
            {
                Id = Plan.Id,
                Name = Plan.Name,
                Description = Plan.Description,
                DurationDays = Plan.DurationDays,
                Price = Plan.Price,
                IsActive = Plan.IsActive
            };
        }

        public UpdatePlanViewModel? GetPlanToUpdate(int planId)
        {
            var Plan = _unitOfWork.GetRepository<Plan>().GetById(planId);
            // Check Active Memberships
            if (Plan is null || Plan.IsActive == false || HasActiveMembership(planId)) return null;
            
            return new UpdatePlanViewModel()
            {
                Name = Plan.Name,
                Description = Plan.Description,
                DurationDays = Plan.DurationDays,
                Price = Plan.Price
            };
        }

        public bool ToggleStatus(int planId)
        {
            var Repository = _unitOfWork.GetRepository<Plan>();

            var Plan = Repository.GetById(planId);
            if (Plan is null || HasActiveMembership(planId)) return false;

            Plan.IsActive = Plan.IsActive == true ? false : true;

            Plan.UpdatedAt = DateTime.Now;
            try
            {
                Repository.Update(Plan);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UpdatePlan(int planId, UpdatePlanViewModel updatedPlan)
        {
            var Plan = _unitOfWork.GetRepository<Plan>().GetById(planId);
            if (Plan is null || Plan.IsActive == false || HasActiveMembership(planId)) return false;

            try
            {
                // Tuples [C# new Feature]
                (Plan.Name, Plan.Description, Plan.DurationDays, Plan.Price, Plan.UpdatedAt) =
                    (updatedPlan.Name, updatedPlan.Description, updatedPlan.DurationDays, updatedPlan.Price, DateTime.Now);

                _unitOfWork.GetRepository<Plan>().Update(Plan);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #region Helper Methods

        private bool HasActiveMembership(int planId)
        {
            var memberships = _unitOfWork.GetRepository<Membership>()
                .GetAll(m => m.PlanId == planId && m.Status == "Active");
            return memberships.Any();
        }

        #endregion
    }
}
