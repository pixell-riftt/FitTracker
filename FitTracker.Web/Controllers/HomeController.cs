using FitTracker.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FitTracker.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction("Index", "Workout");
            }

            return View();
        }

        [Route("Home/Error/{statusCode?}")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int? statusCode)
        {
            if (statusCode == 404)
            {
                return View("NotFound404");
            }

            if (statusCode == 500)
            {
                return View("ServerError500");
            }

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}