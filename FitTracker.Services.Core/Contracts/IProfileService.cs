using FitTracker.ViewModels;

namespace FitTracker.Services.Core.Contracts
{
    public interface IProfileService
    {
        Task<UserProfileViewModel> GetProfileAsync(string userId);

        Task<UserProfileEditViewModel> GetProfileForEditAsync(string userId);

        Task UpdateProfileAsync(string userId, UserProfileEditViewModel model);

        Task EnsureProfileExistsAsync(string userId, string email);
    }
}