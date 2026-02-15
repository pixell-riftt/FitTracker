using FitTracker.Services.Core.Contracts;
using FitTracker.Data;
using FitTracker.ViewModels;
using Microsoft.EntityFrameworkCore;
using FitTracker.Data.Models;

namespace FitTracker.Services.Core
{
    public class WorkoutService : IWorkoutService
    {
        private readonly FitTrackerDbContext _context;

        public WorkoutService(FitTrackerDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<WorkoutIndexViewModel>> GetAllWorkoutsAsync(string? userId)
        {
            return await _context.Workouts
                .Where(w => !w.IsDeleted)
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
                    IsAuthor = userId != null && w.AuthorId == userId,
                    IsSaved = userId != null && w.UsersWorkouts.Any(uw => uw.WorkoutId == w.Id && uw.UserId == userId)
                })
                .ToListAsync();
        }

        public async Task<WorkoutIndexViewModel?> GetWorkoutByIdAsync(int id)
        {
            return await _context.Workouts
                .Include(w => w.ExerciseType)
                .Include(w => w.UsersWorkouts)
                .Where(w => w.Id == id)
                .Select(w => new WorkoutIndexViewModel
                {
                    Id = w.Id,
                    Title = w.Title,
                    ImageUrl = w.ImageUrl,
                    ExerciseType = w.ExerciseType.Name,
                    DurationMinutes = w.DurationMinutes,
                    SavedCount = w.UsersWorkouts.Count,
                    IsAuthor = false,
                    IsSaved = false
                })
                .FirstOrDefaultAsync();
        }

        public async Task<WorkoutDetailsViewModel> GetWorkoutDetailsByIdAsync(int id)
        {
            var workout = await _context.Workouts
                .Include(w => w.ExerciseType)
                .Include(w => w.Author)
                .FirstOrDefaultAsync(w => w.Id == id);

            if (workout == null)
            {
                throw new InvalidOperationException("Workout not found");
            }

            return new WorkoutDetailsViewModel
            {
                Id = workout.Id,
                Title = workout.Title,
                ImageUrl = workout.ImageUrl,
                Notes = workout.Notes,
                DurationMinutes = workout.DurationMinutes,
                ExerciseType = workout.ExerciseType.Name,
                Author = workout.Author.UserName,
                CreatedOn = workout.CreatedOn,
                IsAuthor = false,
                IsSaved = false
            };
        }

        public async Task<bool> IsWorkoutAuthorAsync(int workoutId, string userId)
        {
            return await _context.Workouts
                .AnyAsync(w => w.Id == workoutId && w.AuthorId == userId);
        }

        public async Task<bool> IsWorkoutSavedAsync(int workoutId, string userId)
        {
            return await _context.UsersWorkouts
                .AnyAsync(uw => uw.WorkoutId == workoutId && uw.UserId == userId);
        }

        public async Task<WorkoutCreateViewModel> GetWorkoutCreateViewModelAsync()
        {
            IEnumerable<ExerciseTypeViewModel> exerciseTypes = await _context.ExerciseTypes
                .Select(e => new ExerciseTypeViewModel
                {
                    Id = e.Id,
                    Name = e.Name
                })
                .ToListAsync();

            WorkoutCreateViewModel model = new WorkoutCreateViewModel
            {
                ExerciseTypes = exerciseTypes,
                CreatedOn = DateTime.UtcNow.ToString("yyyy-MM-dd")
            };

            return model;
        }

        public async Task AddWorkoutAsync(WorkoutCreateViewModel model, string authorId)
        {
            if (!DateTime.TryParseExact(model.CreatedOn, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out var createdDate))
            {
                throw new InvalidOperationException("Invalid date format");
            }

            var workout = new Workout
            {
                Title = model.Title,
                Notes = model.Notes,
                DurationMinutes = model.DurationMinutes,
                ImageUrl = model.ImageUrl,
                ExerciseTypeId = model.ExerciseTypeId,
                AuthorId = authorId,
                CreatedOn = createdDate
            };

            _context.Workouts.Add(workout);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<WorkoutFavoritesViewModel>> GetFavoriteWorkoutsAsync(string userId)
        {
            return await _context.UsersWorkouts
                .Where(uw => uw.UserId == userId)
                .Include(uw => uw.Workout)
                .Select(uw => new WorkoutFavoritesViewModel
                {
                    Id = uw.Workout.Id,
                    Title = uw.Workout.Title,
                    ImageUrl = uw.Workout.ImageUrl,
                    ExerciseType = uw.Workout.ExerciseType.Name
                })
                .ToListAsync();
        }

        public async Task SaveWorkoutAsync(int id, string userId)
        {
            if (await _context.UsersWorkouts.AnyAsync(uw => uw.UserId == userId && uw.WorkoutId == id))
            {
                return;
            }

            var userWorkout = new UserWorkout
            {
                WorkoutId = id,
                UserId = userId
            };

            await _context.UsersWorkouts.AddAsync(userWorkout);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveWorkoutAsync(int id, string userId)
        {
            var userWorkout = await _context.UsersWorkouts
                .FirstOrDefaultAsync(uw => uw.UserId == userId && uw.WorkoutId == id);

            if (userWorkout == null)
            {
                return;
            }

            _context.UsersWorkouts.Remove(userWorkout);
            await _context.SaveChangesAsync();
        }

        public async Task<WorkoutEditViewModel> GetWorkoutForEditAsync(int id, string userId)
        {
            var workout = await _context.Workouts
                .Include(w => w.ExerciseType)
                .FirstOrDefaultAsync(w => w.Id == id && !w.IsDeleted);

            if (workout == null)
            {
                throw new ArgumentException("Workout not found.");
            }

            if (workout.AuthorId != userId)
            {
                throw new UnauthorizedAccessException("You are not the author of this workout.");
            }

            return new WorkoutEditViewModel
            {
                Id = workout.Id,
                Title = workout.Title,
                Notes = workout.Notes,
                DurationMinutes = workout.DurationMinutes,
                ImageUrl = workout.ImageUrl,
                CreatedOn = workout.CreatedOn.ToString("yyyy-MM-dd"),
                ExerciseTypeId = workout.ExerciseTypeId,
                ExerciseTypes = await _context.ExerciseTypes
                    .Select(e => new ExerciseTypeViewModel
                    {
                        Id = e.Id,
                        Name = e.Name
                    })
                    .ToListAsync()
            };
        }

        public async Task EditWorkoutAsync(WorkoutEditViewModel model, string userId)
        {
            var workout = await _context.Workouts.FirstOrDefaultAsync(w => w.Id == model.Id && !w.IsDeleted);

            if (workout == null || workout.AuthorId != userId)
            {
                throw new UnauthorizedAccessException("You are not authorized to edit this workout.");
            }

            workout.Title = model.Title;
            workout.Notes = model.Notes;
            workout.DurationMinutes = model.DurationMinutes;
            workout.ImageUrl = model.ImageUrl;
            workout.CreatedOn = DateTime.Parse(model.CreatedOn);
            workout.ExerciseTypeId = model.ExerciseTypeId;

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ExerciseTypeViewModel>> GetAllExerciseTypesAsync()
        {
            return await _context.ExerciseTypes
                .Select(e => new ExerciseTypeViewModel
                {
                    Id = e.Id,
                    Name = e.Name
                })
                .ToListAsync();
        }

        public async Task DeleteWorkoutAsync(int id, string userId)
        {
            var workout = await _context.Workouts
                .FirstOrDefaultAsync(w => w.Id == id && !w.IsDeleted);

            if (workout == null)
            {
                throw new ArgumentException("Workout not found.");
            }

            if (workout.AuthorId != userId)
            {
                throw new UnauthorizedAccessException("You are not authorized to delete this workout.");
            }

            workout.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        public async Task<WorkoutDeleteViewModel> GetWorkoutDeleteDetailsAsync(int id, string userId)
        {
            var workout = await _context.Workouts
                .Include(w => w.ExerciseType)
                .Include(w => w.Author)
                .FirstOrDefaultAsync(w => w.Id == id && !w.IsDeleted);

            if (workout == null)
            {
                throw new ArgumentException("Workout not found.");
            }

            if (workout.AuthorId != userId)
            {
                throw new UnauthorizedAccessException("You are not the author of this workout.");
            }

            return new WorkoutDeleteViewModel
            {
                Id = workout.Id,
                Title = workout.Title,
                AuthorId = workout.AuthorId,
                Author = workout.Author.UserName
            };
        }

        public async Task<IEnumerable<WorkoutIndexViewModel>> GetMyWorkoutsAsync(string userId)
        {
            return await _context.Workouts
                .Where(w => !w.IsDeleted && w.AuthorId == userId)
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
        }

    }
}