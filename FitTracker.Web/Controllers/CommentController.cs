using FitTracker.Services.Core.Contracts;
using FitTracker.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitTracker.Web.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        private readonly IWorkoutService _workoutService;

        public CommentController(IWorkoutService workoutService)
        {
            _workoutService = workoutService;
        }

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(CommentCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Comment must be between 2 and 500 characters.";
                return RedirectToAction("Details", "Workout", new { id = model.WorkoutId });
            }

            await _workoutService.AddCommentAsync(model, GetUserId());
            TempData["Success"] = "Comment added!";
            return RedirectToAction("Details", "Workout", new { id = model.WorkoutId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, int workoutId)
        {
            try
            {
                bool isAdmin = User.IsInRole("Admin");
                await _workoutService.DeleteCommentAsync(id, GetUserId(), isAdmin);
                TempData["Success"] = "Comment deleted!";
            }
            catch
            {
                TempData["Error"] = "Could not delete comment.";
            }

            return RedirectToAction("Details", "Workout", new { id = workoutId });
        }
    }
}