using FitTracker.Services.Core.Contracts;
using FitTracker.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitTracker.Data;

namespace FitTracker.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly FitTrackerDbContext _context;

        public AdminController(IAdminService adminService, FitTrackerDbContext context)
        {
            _adminService = adminService;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            var model = await _adminService.GetDashboardAsync();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ManageUsers()
        {
            var users = await _adminService.GetAllUsersAsync();
            return View(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PromoteUser(string userId)
        {
            await _adminService.PromoteToAdminAsync(userId);
            TempData["Success"] = "User promoted to Admin!";
            return RedirectToAction(nameof(ManageUsers));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DemoteUser(string userId)
        {
            await _adminService.DemoteFromAdminAsync(userId);
            TempData["Success"] = "User demoted from Admin!";
            return RedirectToAction(nameof(ManageUsers));
        }

        [HttpGet]
        public async Task<IActionResult> ManageWorkouts()
        {
            var workouts = await _context.Workouts
                .Where(w => !w.IsDeleted)
                .Include(w => w.ExerciseType)
                .Include(w => w.Author)
                .OrderByDescending(w => w.CreatedOn)
                .Select(w => new WorkoutIndexViewModel
                {
                    Id = w.Id,
                    Title = w.Title,
                    ImageUrl = w.ImageUrl,
                    ExerciseType = w.ExerciseType.Name,
                    DurationMinutes = w.DurationMinutes,
                    SavedCount = w.UsersWorkouts.Count,
                    IsAuthor = true,
                    IsSaved = false
                })
                .ToListAsync();

            return View(workouts);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteWorkout(int id)
        {
            await _adminService.DeleteWorkoutAsAdminAsync(id);
            TempData["Success"] = "Workout deleted!";
            return RedirectToAction(nameof(ManageWorkouts));
        }

        [HttpGet]
        public async Task<IActionResult> ManageComments()
        {
            var comments = await _context.Comments
                .Include(c => c.Author)
                .Include(c => c.Workout)
                .OrderByDescending(c => c.CreatedOn)
                .Select(c => new CommentViewModel
                {
                    Id = c.Id,
                    Content = c.Content,
                    AuthorName = c.Author.UserName,
                    AuthorId = c.AuthorId,
                    CreatedOn = c.CreatedOn
                })
                .ToListAsync();

            return View(comments);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteComment(int id)
        {
            await _adminService.DeleteCommentAsAdminAsync(id);
            TempData["Success"] = "Comment deleted!";
            return RedirectToAction(nameof(ManageComments));
        }
    }
}