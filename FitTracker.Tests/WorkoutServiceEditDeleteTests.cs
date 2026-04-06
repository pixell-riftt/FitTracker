using FitTracker.Services.Core;
using FitTracker.ViewModels;
using Moq;
using Xunit;

namespace FitTracker.Tests
{
    public class WorkoutServiceEditDeleteTests
    {
        [Fact]
        public async Task EditWorkoutAsync_UpdatesWorkoutCorrectly()
        {
            var context = TestDbHelper.GetInMemoryDbContext();
            var service = new WorkoutService(context);

            var model = new WorkoutEditViewModel
            {
                Id = 1,
                Title = "Updated Run",
                Notes = "Updated notes for the run.",
                DurationMinutes = 45,
                ExerciseTypeId = 1,
                CreatedOn = "2026-01-15"
            };

            await service.EditWorkoutAsync(model, "user1");

            var updated = await service.GetWorkoutDetailsByIdAsync(1);
            Assert.Equal("Updated Run", updated.Title);
            Assert.Equal(45, updated.DurationMinutes);
        }

        [Fact]
        public async Task EditWorkoutAsync_ThrowsForWrongUser()
        {
            var context = TestDbHelper.GetInMemoryDbContext();
            var service = new WorkoutService(context);

            var model = new WorkoutEditViewModel
            {
                Id = 1,
                Title = "Hacked Title",
                Notes = "Trying to edit someone elses workout.",
                DurationMinutes = 10,
                ExerciseTypeId = 1,
                CreatedOn = "2026-01-15"
            };

            await Assert.ThrowsAsync<UnauthorizedAccessException>(
                () => service.EditWorkoutAsync(model, "user2"));
        }

        [Fact]
        public async Task DeleteWorkoutAsync_SoftDeletesWorkout()
        {
            var context = TestDbHelper.GetInMemoryDbContext();
            var service = new WorkoutService(context);

            await service.DeleteWorkoutAsync(1, "user1");

            var workouts = await service.GetAllWorkoutsAsync("user1");
            Assert.Single(workouts);
        }

        [Fact]
        public async Task DeleteWorkoutAsync_ThrowsForWrongUser()
        {
            var context = TestDbHelper.GetInMemoryDbContext();
            var service = new WorkoutService(context);

            await Assert.ThrowsAsync<UnauthorizedAccessException>(
                () => service.DeleteWorkoutAsync(1, "user2"));
        }

        [Fact]
        public async Task DeleteWorkoutAsync_ThrowsForInvalidId()
        {
            var context = TestDbHelper.GetInMemoryDbContext();
            var service = new WorkoutService(context);

            await Assert.ThrowsAsync<ArgumentException>(
                () => service.DeleteWorkoutAsync(999, "user1"));
        }

        [Fact]
        public async Task GetWorkoutForEditAsync_ReturnsCorrectData()
        {
            var context = TestDbHelper.GetInMemoryDbContext();
            var service = new WorkoutService(context);

            var result = await service.GetWorkoutForEditAsync(1, "user1");

            Assert.Equal("Morning Run", result.Title);
            Assert.Equal(30, result.DurationMinutes);
            Assert.NotNull(result.ExerciseTypes);
        }

        [Fact]
        public async Task GetWorkoutForEditAsync_ThrowsForWrongUser()
        {
            var context = TestDbHelper.GetInMemoryDbContext();
            var service = new WorkoutService(context);

            await Assert.ThrowsAsync<UnauthorizedAccessException>(
                () => service.GetWorkoutForEditAsync(1, "user2"));
        }

        [Fact]
        public async Task GetWorkoutDeleteDetailsAsync_ThrowsForWrongUser()
        {
            var context = TestDbHelper.GetInMemoryDbContext();
            var service = new WorkoutService(context);

            await Assert.ThrowsAsync<UnauthorizedAccessException>(
                () => service.GetWorkoutDeleteDetailsAsync(1, "user2"));
        }
    }
}
