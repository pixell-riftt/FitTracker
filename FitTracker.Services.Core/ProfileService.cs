using FitTracker.Data;
using FitTracker.Data.Models;
using FitTracker.Services.Core.Contracts;
using FitTracker.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace FitTracker.Services.Core
{
    public class ProfileService : IProfileService
    {
        private readonly FitTrackerDbContext _context;

        public ProfileService(FitTrackerDbContext context)
        {
            _context = context;
        }

        public async Task EnsureProfileExistsAsync(string userId, string email)
        {
            bool exists = await _context.UserProfiles.AnyAsync(up => up.UserId == userId);

            if (!exists)
            {
                var profile = new UserProfile
                {
                    UserId = userId,
                    DisplayName = email.Split('@')[0]
                };

                _context.UserProfiles.Add(profile);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<UserProfileViewModel> GetProfileAsync(string userId)
        {
            var profile = await _context.UserProfiles
                .Include(up => up.User)
                .FirstOrDefaultAsync(up => up.UserId == userId);

            var workouts = await _context.Workouts
                .Where(w => w.AuthorId == userId && !w.IsDeleted)
                .Include(w => w.ExerciseType)
                .Include(w => w.UsersWorkouts)
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

            return new UserProfileViewModel
            {
                UserId = userId,
                DisplayName = profile?.DisplayName ?? "Unknown",
                Bio = profile?.Bio,
                ProfileImageUrl = profile?.ProfileImageUrl,
                Email = profile?.User?.Email ?? "Unknown",
                WorkoutCount = workouts.Count,
                TotalMinutes = workouts.Sum(w => w.DurationMinutes),
                Workouts = workouts
            };
        }

        public async Task<UserProfileEditViewModel> GetProfileForEditAsync(string userId)
        {
            var profile = await _context.UserProfiles
                .FirstOrDefaultAsync(up => up.UserId == userId);

            if (profile == null)
            {
                throw new ArgumentException("Profile not found.");
            }

            return new UserProfileEditViewModel
            {
                DisplayName = profile.DisplayName,
                Bio = profile.Bio,
                ProfileImageUrl = profile.ProfileImageUrl
            };
        }

        public async Task UpdateProfileAsync(string userId, UserProfileEditViewModel model)
        {
            var profile = await _context.UserProfiles
                .FirstOrDefaultAsync(up => up.UserId == userId);

            if (profile == null)
            {
                throw new ArgumentException("Profile not found.");
            }

            profile.DisplayName = model.DisplayName;
            profile.Bio = model.Bio;
            profile.ProfileImageUrl = model.ProfileImageUrl;

            await _context.SaveChangesAsync();
        }
    }
}