using FitTracker.ViewModels;

namespace FitTracker.Services.Core.Contracts
{
    public interface IAdminService
    {
        Task<AdminDashboardViewModel> GetDashboardAsync();

        Task<IEnumerable<AdminUserViewModel>> GetAllUsersAsync();

        Task PromoteToAdminAsync(string userId);

        Task DemoteFromAdminAsync(string userId);

        Task DeleteWorkoutAsAdminAsync(int workoutId);

        Task DeleteCommentAsAdminAsync(int commentId);
    }
}