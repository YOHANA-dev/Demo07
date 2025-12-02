using GymSystemBLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymSystemPL.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAnalyticsService _analyticsService;

        public HomeController(IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        // BaseURL/Home/Index
        public IActionResult Index()
        {
            var Data = _analyticsService.GetAnalyticsData();
            return View(Data);
        }
    }
}
