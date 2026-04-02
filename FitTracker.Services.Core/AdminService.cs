using FitTracker.Data;
using FitTracker.Services.Core.Contracts;
using FitTracker.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FitTracker.Services.Core
{
    public class AdminService : IAdminService
    {
        private readonly FitTrackerDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public AdminService(FitTrackerDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<AdminDashboardViewModel> GetDashboardAsync()
        {
            return new AdminDashboardViewModel
            {
                TotalUsers = await _context.Users.CountAsync(),
                TotalWorkouts = await _context.Workouts.CountAsync(w => !w.IsDeleted),
                TotalComments = await _context.Comments.CountAsync(),
                TotalExerciseTypes = await _context.ExerciseTypes.CountAsync()
            };
        }

        public async Task<IEnumerable<AdminUserViewModel>> GetAllUsersAsync()
        {
            var users = await _context.Users.ToListAsync();
            var result = new List<AdminUserViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var profile = await _context.UserProfiles
                    .FirstOrDefaultAsync(up => up.UserId == user.Id);

                result.Add(new AdminUserViewModel
                {
                    Id = user.Id,
                    Email = user.Email ?? "Unknown",
                    DisplayName = profile?.DisplayName,
                    IsAdmin = roles.Contains("Admin"),
                    WorkoutCount = await _context.Workouts.CountAsync(w => w.AuthorId == user.Id && !w.IsDeleted)
                });
            }

            return result;
        }

        public async Task PromoteToAdminAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                await _userManager.AddToRoleAsync(user, "Admin");
            }
        }

        public async Task DemoteFromAdminAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                await _userManager.RemoveFromRoleAsync(user, "Admin");
            }
        }

        public async Task DeleteWorkoutAsAdminAsync(int workoutId)
        {
            var workout = await _context.Workouts
                .FirstOrDefaultAsync(w => w.Id == workoutId && !w.IsDeleted);

            if (workout != null)
            {
                workout.IsDeleted = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteCommentAsAdminAsync(int commentId)
        {
            var comment = await _context.Comments
                .FirstOrDefaultAsync(c => c.Id == commentId);

            if (comment != null)
            {
                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();
            }
        }
    }
}