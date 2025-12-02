using GymSystemBLL.Services.Interfaces;
using GymSystemBLL.ViewModels.BookingViewModels;
using GymSystemDAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymSystemPL.Controllers
{
    public class BookingController(IBookingService _bookingService) : Controller
    {
        public ActionResult Index()
        {
            var sessions = _bookingService.GetAllSessionsWithTrainerAndCategories();
            return View(sessions);
        }

        public ActionResult GetMembersForUpcomingSession(int id)
        {
            var members = _bookingService.GetAllMembersForSession(id);
            ViewBag.sessionId = id;
            return View(members);
        }
        public ActionResult GetMembersForOngoingSession(int id)
        {
            var members = _bookingService.GetAllMembersForSession(id);
            ViewBag.SessionId = id;
            return View(members);
        }

        [HttpPost]
        public ActionResult MarkAttendance(int sessionId, int memberId)
        {
            _bookingService.MarkMemberAttendance(memberId, sessionId);
            return RedirectToAction("GetMembersForOngoingSession", new { id = sessionId });
        }

        public ActionResult Create(int id)
        {
            var members = _bookingService.GetMembersForDropDown(id);
            var membersSelectList = new SelectList(members, "Id", "Name");
            ViewBag.Members = membersSelectList;
            ViewBag.SessionId = id;
            return View();
        }

        [HttpPost]
        public ActionResult Create(CreateBookingViewModel model)
        {
            var Result = _bookingService.CreateBooking(model);
            if (Result)
            {
                TempData["SuccessMessage"] = "Booking Create Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed To Create Booking";
            }
            return RedirectToAction(nameof(GetMembersForUpcomingSession), new { id = model.SessionId });
        }

        [HttpPost]
        public ActionResult Cancel(int memberId, int sessionId)
        {
            var Result = _bookingService.DeleteBooking(memberId, sessionId);
            if (Result)
            {
                TempData["SuccessMessage"] = "Booking Cancelled Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed To Cancel Booking";
            }
            return RedirectToAction(nameof(GetMembersForUpcomingSession), new { id = sessionId });
        }
    }
}
