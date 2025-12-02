using GymSystemBLL.Services.Interfaces;
using GymSystemBLL.ViewModels.MemberViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymSystemPL.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class MemberController : Controller
    {
        private readonly IMemberService _memberService;

        // Ask CLR to inject Object from MemberService
        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        } // Register Service in Program.cs

        #region Get All Members

        public IActionResult Index()
        {
            var members = _memberService.GetAllMembers();
            return View(members);
        }

        #endregion

        #region Get Member Details

        public ActionResult MemberDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id cannot be 0 or Negative Number !";
                return RedirectToAction(nameof(Index));
            }

            var memberDetails = _memberService.GetMemberDetails(id);
            if (memberDetails == null)
            {
                TempData["ErrorMessage"] = "Member Not Found !";
                return RedirectToAction(nameof(Index));
            }
            return View(memberDetails);
        }

        #endregion

        #region Get Health Record Details

        public ActionResult HealthRecordDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id cannot be 0 or Negative Number !";
                return RedirectToAction(nameof(Index));
            }
            var healthRecordDetails = _memberService.GetMemberHealthRecordDetails(id);
            if (healthRecordDetails == null)
            {
                TempData["ErrorMessage"] = "Member Not Found !";
                return RedirectToAction(nameof(Index));
            }
            return View(healthRecordDetails);
        }

        #endregion

        #region Create Member

        public ActionResult Create()
        {
            return View();
        }

        // Add to DB
        [HttpPost]
        public ActionResult CreateMember(CreateMemberViewModel createdMember)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInvalid", "Check Data and Missing Fields !");
                return View("Create", createdMember);
            }

            bool Result = _memberService.CreateMembers(createdMember);
            if (Result)
            {
                TempData["SuccessMessage"] = "Member Created Successfully !";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to Create Member !";
            }

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Edit Member

        public ActionResult MemberEdit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id cannot be 0 or Negative Number !";
                return RedirectToAction(nameof(Index));
            }
            var Member = _memberService.GetMemberToUpdate(id);
            if (Member == null)
            {
                TempData["ErrorMessage"] = "Member Not Found !";
                return RedirectToAction(nameof(Index));
            }
            return View(Member);
        }

        [HttpPost]
        public ActionResult MemberEdit([FromRoute] int id, MemberToUpdateViewModel updatedMember)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInvalid", "Check Data and Missing Fields !");
                return View("MemberEdit", updatedMember);
            }

            bool Result = _memberService.UpdateMemberDetails(id, updatedMember);
            if (Result)
            {
                TempData["SuccessMessage"] = "Member Updated Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed To Update Member !";
            }
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Delete Member

        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Cannot be 0 or Negative Number !";
                return RedirectToAction(nameof(Index));
            }

            var Member = _memberService.GetMemberDetails(id);
            if(Member == null)
            {
                TempData["ErrorMessage"] = "Member Not Found !";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.MemberId = id;
            ViewBag.MemberName = Member.Name;
            return View(Member);
        }

        public ActionResult DeleteConfirmed([FromForm]int id)
        {
            var Result = _memberService.RemoveMember(id);
            if (Result)
            {
                TempData["SuccessMessage"] = "Member Deleted Successfully !";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed To Delete Member !";
            }
            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}
