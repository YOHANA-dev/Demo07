using GymSystemBLL.Services.Classes;
using GymSystemBLL.Services.Interfaces;
using GymSystemBLL.ViewModels.SessionViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymSystemPL.Controllers
{
    public class SessionController : Controller
    {
        private readonly ISessionService _sessionService;

        public SessionController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        #region Get Sessions

        public IActionResult Index()
        {
            var Sessions = _sessionService.GetAllSessions();
            return View(Sessions);
        }

        #endregion

        #region Get Session Details

        public ActionResult Details(int id)
        {
            if(id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Session Id";
                return RedirectToAction("Index");
            }

            var session = _sessionService.GetSessionById(id);
            if(session == null)
            {
                TempData["ErrorMessage"] = "Session Not Found";
                return RedirectToAction("Index");
            }

            return View(session);
        }

        #endregion

        #region Create Session

        public ActionResult Create()
        {
            LoadDropDowns();
            return View();
        }

        [HttpPost]
        public ActionResult Create(CreateSessionViewModel createdSession)
        {
            if (!ModelState.IsValid)
            {
                LoadDropDowns();
                return View(createdSession);
            }

            var Result = _sessionService.CreateSession(createdSession);
            if (!Result)
            {
                TempData["ErrorMessage"] = "Failed to Create Session";
                LoadDropDowns();
                return View(createdSession);
            }
            else
            {
                TempData["SuccessMessage"] = "Session Created Successfully";
                LoadDropDowns();
                return RedirectToAction("Index");
            }
        }

        #endregion

        #region Edit Session

        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Session Id";
                return RedirectToAction("Index");
            }
            var Session = _sessionService.GetSessionToUpdate(id);
            if (Session == null)
            {
                TempData["ErrorMessage"] = "Session Not Found !";
                return RedirectToAction("Index");
            }

            LoadDropDownForTrainers();
            return View(Session);
        }

        [HttpPost]
        public ActionResult Edit([FromRoute]int id,UpdateSessionViewModel updatedSession)
        {
            if (!ModelState.IsValid)
            {
                LoadDropDownForTrainers();
                return View(updatedSession);
            }

            var Result = _sessionService.UpdateSession(updatedSession,id);
            if (Result)
            {
                TempData["SuccessMessage"] = "Session Updated Successfully";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = "Failed To Update Session";
                LoadDropDownForTrainers();
                return View(updatedSession);
            }
        }

        #endregion

        #region Delete Session

        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Session Id";
                return RedirectToAction("Index");
            }
            var Session = _sessionService.GetSessionById(id);
            if(Session is null)
            {
                TempData["ErrorMessage"] = "Session Not Found !";
                return RedirectToAction("Index");
            }
            ViewBag.SessionId = id;
            return View(Session);
        }

        [HttpPost]
        public ActionResult DeleteConfirmed(int id)
        {
            var Result = _sessionService.RemoveSession(id);
            if (Result)
            {
                TempData["SuccessMessage"] = "Session Deleted Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed To Delete Session";
            }
            return RedirectToAction("Index");
        }

        #endregion

        #region Helper Methods

        private void LoadDropDowns()
        {
            LoadDropDownForTrainers();
            LoadDropDownForCategories();
        }
        private void LoadDropDownForTrainers()
        {
            var Trainers = _sessionService.GetTrainerForSessions();
            ViewBag.Trainers = new SelectList(Trainers, "Id", "Name");
        }
        private void LoadDropDownForCategories()
        {
            var Categories = _sessionService.GetCategoryForSessions();
            ViewBag.Categories = new SelectList(Categories, "Id", "Name");
        }

        #endregion
    }
}
