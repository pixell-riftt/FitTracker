using FitTracker.Services.Core;
using Moq;
using Xunit;

namespace FitTracker.Tests
{
    public class WorkoutServiceFavoritesTests
    {
        [Fact]
        public async Task SaveWorkoutAsync_AddsToFavorites()
        {
            var context = TestDbHelper.GetInMemoryDbContext();
            var service = new WorkoutService(context);

            await service.SaveWorkoutAsync(1, "user2");

            var isSaved = await service.IsWorkoutSavedAsync(1, "user2");
            Assert.True(isSaved);
        }

        [Fact]
        public async Task SaveWorkoutAsync_DoesNotDuplicateSave()
        {
            var context = TestDbHelper.GetInMemoryDbContext();
            var service = new WorkoutService(context);

            await service.SaveWorkoutAsync(1, "user2");
            await service.SaveWorkoutAsync(1, "user2");

            var favorites = await service.GetFavoriteWorkoutsAsync("user2");
            Assert.Single(favorites);
        }

        [Fact]
        public async Task RemoveWorkoutAsync_RemovesFromFavorites()
        {
            var context = TestDbHelper.GetInMemoryDbContext();
            var service = new WorkoutService(context);

            await service.SaveWorkoutAsync(1, "user2");
            await service.RemoveWorkoutAsync(1, "user2");

            var isSaved = await service.IsWorkoutSavedAsync(1, "user2");
            Assert.False(isSaved);
        }

        [Fact]
        public async Task RemoveWorkoutAsync_DoesNothingIfNotSaved()
        {
            var context = TestDbHelper.GetInMemoryDbContext();
            var service = new WorkoutService(context);

            await service.RemoveWorkoutAsync(1, "user2");

            var favorites = await service.GetFavoriteWorkoutsAsync("user2");
            Assert.Empty(favorites);
        }

        [Fact]
        public async Task GetFavoriteWorkoutsAsync_ReturnsOnlySavedWorkouts()
        {
            var context = TestDbHelper.GetInMemoryDbContext();
            var service = new WorkoutService(context);

            await service.SaveWorkoutAsync(1, "user2");
            await service.SaveWorkoutAsync(2, "user2");

            var favorites = await service.GetFavoriteWorkoutsAsync("user2");
            Assert.Equal(2, favorites.Count());
        }

        [Fact]
        public async Task IsWorkoutAuthorAsync_ReturnsTrueForAuthor()
        {
            var context = TestDbHelper.GetInMemoryDbContext();
            var service = new WorkoutService(context);

            var result = await service.IsWorkoutAuthorAsync(1, "user1");
            Assert.True(result);
        }

        [Fact]
        public async Task IsWorkoutAuthorAsync_ReturnsFalseForNonAuthor()
        {
            var context = TestDbHelper.GetInMemoryDbContext();
            var service = new WorkoutService(context);

            var result = await service.IsWorkoutAuthorAsync(1, "user2");
            Assert.False(result);
        }

        [Fact]
        public async Task GetMyWorkoutsAsync_ReturnsOnlyUserWorkouts()
        {
            var context = TestDbHelper.GetInMemoryDbContext();
            var service = new WorkoutService(context);

            var result = await service.GetMyWorkoutsAsync("user1");
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetMyWorkoutsAsync_ReturnsEmptyForUserWithNoWorkouts()
        {
            var context = TestDbHelper.GetInMemoryDbContext();
            var service = new WorkoutService(context);

            var result = await service.GetMyWorkoutsAsync("user2");
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllExerciseTypesAsync_ReturnsAllTypes()
        {
            var context = TestDbHelper.GetInMemoryDbContext();
            var service = new WorkoutService(context);

            var result = await service.GetAllExerciseTypesAsync();
            Assert.Equal(3, result.Count());
        }
    }
}
