using FitTracker.Services.Core.Contracts;
using FitTracker.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitTracker.Web.Controllers
{
    [Authorize]
    public class WorkoutController : Controller
    {
        private readonly IWorkoutService _workoutService;

        public WorkoutController(IWorkoutService workoutService)
        {
            _workoutService = workoutService;
        }

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var workouts = await _workoutService.GetAllWorkoutsAsync(GetUserId());
            return View(workouts);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var model = await _workoutService.GetWorkoutDetailsByIdAsync(id);
            string userId = GetUserId();

            model.IsAuthor = await _workoutService.IsWorkoutAuthorAsync(id, userId);
            model.IsSaved = await _workoutService.IsWorkoutSavedAsync(id, userId);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = await _workoutService.GetWorkoutCreateViewModelAsync();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WorkoutCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.ExerciseTypes = await _workoutService.GetAllExerciseTypesAsync();
                return View(model);
            }

            await _workoutService.AddWorkoutAsync(model, GetUserId());
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var model = await _workoutService.GetWorkoutForEditAsync(id, GetUserId());
                return View(model);
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(WorkoutEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.ExerciseTypes = await _workoutService.GetAllExerciseTypesAsync();
                return View(model);
            }

            try
            {
                await _workoutService.EditWorkoutAsync(model, GetUserId());
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var model = await _workoutService.GetWorkoutDeleteDetailsAsync(id, GetUserId());
                return View(model);
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _workoutService.DeleteWorkoutAsync(id, GetUserId());
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(int id)
        {
            await _workoutService.SaveWorkoutAsync(id, GetUserId());
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(int id)
        {
            await _workoutService.RemoveWorkoutAsync(id, GetUserId());
            return RedirectToAction(nameof(Favorites));
        }

        [HttpGet]
        public async Task<IActionResult> Favorites()
        {
            var favorites = await _workoutService.GetFavoriteWorkoutsAsync(GetUserId());
            return View(favorites);
        }
    }
}