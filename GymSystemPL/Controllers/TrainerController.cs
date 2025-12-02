using GymSystemBLL.Services.Classes;
using GymSystemBLL.Services.Interfaces;
using GymSystemBLL.ViewModels.MemberViewModels;
using GymSystemBLL.ViewModels.TrainerViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymSystemPL.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class TrainerController : Controller
    {
        private readonly ITrainerService _trainerService;

        public TrainerController(ITrainerService trainerService)
        {
            _trainerService = trainerService;
        }

        #region Get All Trainers

        public IActionResult Index()
        {
            var trainers = _trainerService.GetAllTrainers();
            return View(trainers);
        }

        #endregion

        #region Get Trainer Details

        public ActionResult TrainerDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id cannot be 0 or Negative Number !";
                return RedirectToAction(nameof(Index));
            }

            var trainerDetails = _trainerService.GetTrainerDetails(id);
            if (trainerDetails == null)
            {
                TempData["ErrorMessage"] = "Trainer Not Found !";
                return RedirectToAction(nameof(Index));
            }
            return View(trainerDetails);
        }

        #endregion

        #region Create Trainer

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateTrainer(CreateTrainerViewModel createdTrainer)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInvalid", "Check Data and Missing Fields !");
                return View("Create", createdTrainer);
            }

            bool Result = _trainerService.CreateTrainers(createdTrainer);
            if (Result)
            {
                TempData["SuccessMessage"] = "Trainer Created Successfully !";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to Create Trainer !";
            }

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Edit Trainer

        public ActionResult TrainerEdit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id cannot be 0 or Negative Number !";
                return RedirectToAction(nameof(Index));
            }
            var Trainer = _trainerService.GetTrainerToUpdate(id);
            if (Trainer == null)
            {
                TempData["ErrorMessage"] = "Trainer Not Found !";
                return RedirectToAction(nameof(Index));
            }
            return View(Trainer);
        }

        [HttpPost]
        public ActionResult TrainerEdit([FromRoute] int id, TrainerToUpdateViewModel updatedTrainer)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInvalid", "Check Data and Missing Fields !");
                return View("TrainerEdit", updatedTrainer);
            }

            bool Result = _trainerService.UpdateTrainerDetails(id, updatedTrainer);
            if (Result)
            {
                TempData["SuccessMessage"] = "Trainer Updated Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed To Update Trainer !";
            }
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Delete Trainer

        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Cannot be 0 or Negative Number !";
                return RedirectToAction(nameof(Index));
            }

            var Trainer = _trainerService.GetTrainerDetails(id);
            if (Trainer == null)
            {
                TempData["ErrorMessage"] = "Trainer Not Found !";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.TrainerId = id;
            return View(Trainer);
        }

        public ActionResult DeleteConfirmed([FromForm] int id)
        {
            var Result = _trainerService.RemoveTrainer(id);
            if (Result)
            {
                TempData["SuccessMessage"] = "Trainer Deleted Successfully !";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed To Delete Trainer !";
            }
            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}
