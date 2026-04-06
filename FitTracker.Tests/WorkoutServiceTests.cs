using FitTracker.Services.Core;
using FitTracker.ViewModels;
using Moq;
using Xunit;

namespace FitTracker.Tests
{
    public class WorkoutServiceTests
    {
        [Fact]
        public async Task GetAllWorkoutsAsync_ReturnsOnlyNonDeletedWorkouts()
        {
            var context = TestDbHelper.GetInMemoryDbContext();
            var service = new WorkoutService(context);

            var result = await service.GetAllWorkoutsAsync("user1");

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetAllWorkoutsAsync_SetsIsAuthorCorrectly()
        {
            var context = TestDbHelper.GetInMemoryDbContext();
            var service = new WorkoutService(context);

            var result = (await service.GetAllWorkoutsAsync("user1")).ToList();

            Assert.All(result, w => Assert.True(w.IsAuthor));
        }

        [Fact]
        public async Task GetAllWorkoutsAsync_SetsIsAuthorFalseForOtherUser()
        {
            var context = TestDbHelper.GetInMemoryDbContext();
            var service = new WorkoutService(context);

            var result = (await service.GetAllWorkoutsAsync("user2")).ToList();

            Assert.All(result, w => Assert.False(w.IsAuthor));
        }

        [Fact]
        public async Task GetWorkoutDetailsByIdAsync_ReturnsCorrectWorkout()
        {
            var context = TestDbHelper.GetInMemoryDbContext();
            var service = new WorkoutService(context);

            var result = await service.GetWorkoutDetailsByIdAsync(1);

            Assert.Equal("Morning Run", result.Title);
            Assert.Equal(30, result.DurationMinutes);
            Assert.Equal("Cardio", result.ExerciseType);
        }

        [Fact]
        public async Task GetWorkoutDetailsByIdAsync_ThrowsForInvalidId()
        {
            var context = TestDbHelper.GetInMemoryDbContext();
            var service = new WorkoutService(context);

            await Assert.ThrowsAsync<InvalidOperationException>(
                () => service.GetWorkoutDetailsByIdAsync(999));
        }

        [Fact]
        public async Task AddWorkoutAsync_AddsWorkoutToDatabase()
        {
            var context = TestDbHelper.GetInMemoryDbContext();
            var service = new WorkoutService(context);

            var model = new WorkoutCreateViewModel
            {
                Title = "New Workout",
                Notes = "Brand new workout session.",
                DurationMinutes = 40,
                ExerciseTypeId = 1,
                CreatedOn = "2026-02-01"
            };

            await service.AddWorkoutAsync(model, "user1");

            var workouts = await service.GetAllWorkoutsAsync("user1");
            Assert.Equal(3, workouts.Count());
        }

        [Fact]
        public async Task AddWorkoutAsync_ThrowsForInvalidDate()
        {
            var context = TestDbHelper.GetInMemoryDbContext();
            var service = new WorkoutService(context);

            var model = new WorkoutCreateViewModel
            {
                Title = "Bad Date Workout",
                Notes = "This has a bad date format.",
                DurationMinutes = 30,
                ExerciseTypeId = 1,
                CreatedOn = "not-a-date"
            };

            await Assert.ThrowsAsync<InvalidOperationException>(
                () => service.AddWorkoutAsync(model, "user1"));
        }
    }
}
