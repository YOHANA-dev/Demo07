using GymSystemBLL.Services.Interfaces;
using GymSystemBLL.ViewModels.PlanViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;

namespace GymSystemPL.Controllers
{
    public class PlanController : Controller
    {
        private readonly IPlanService _planService;

        public PlanController(IPlanService planService)
        {
            _planService = planService;
        } // Register Service in Program.cs

        #region Get All Plans

        public IActionResult Index()
        {
            var Plans = _planService.GetAllPlans();
            return View(Plans);
        }

        #endregion

        #region Get Plan Details

        public ActionResult Details(int id)
        {
            if(id <= 0)
            {
                TempData["ErrorMessage"] = "Id Cannot Be 0 or Negative Number !";
                return RedirectToAction("Index");
            }
            var Plan = _planService.GetPlanById(id);
            if(Plan == null)
            {
                TempData["ErrorMessage"] = "Plan Not Found !";
                return RedirectToAction("Index");
            }
            return View(Plan);
        }

        #endregion

        #region Edit Plan

        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Cannot Be 0 or Negative Number !";
                return RedirectToAction("Index");
            }
            var Plan = _planService.GetPlanToUpdate(id);
            if (Plan == null)
            {
                TempData["ErrorMessage"] = "Plan Not Found !";
                return RedirectToAction("Index");
            }
            return View(Plan);
        }

        [HttpPost]
        public ActionResult Edit([FromRoute]int id,UpdatePlanViewModel updatedPlan)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("WrongData", "Check Data Again !");
                return View(updatedPlan);
            }
            var Result = _planService.UpdatePlan(id, updatedPlan);
            if (!Result)
            {
                TempData["ErrorMessage"] = "Failed To Update Plan !";
                return RedirectToAction(nameof(Index));
            }

            TempData["SuccessMessage"] = "Plan Updated Successfully !";
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Soft Delete - Active/Inactive Plan

        [HttpPost]
        public ActionResult Activate(int id)
        {
            var Result = _planService.ToggleStatus(id);
            if (Result)
            {
                TempData["SuccessMessage"] = "Plan Status Changed Successfully !";
                return RedirectToAction(nameof(Index));
            } 
            else
            {
                TempData["ErrorMessage"] = "Failed To Change Plan Status !";
                return RedirectToAction(nameof(Index));
            }
        }

        #endregion
    }
}
