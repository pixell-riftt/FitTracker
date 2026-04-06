using FitTracker.Services.Core;
using FitTracker.ViewModels;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FitTracker.Tests
{
    public class ProfileServiceTests
    {
        [Fact]
        public async Task GetProfileAsync_ReturnsCorrectProfile()
        {
            var context = TestDbHelper.GetInMemoryDbContext();
            var service = new ProfileService(context);

            var profile = await service.GetProfileAsync("user1");

            Assert.Equal("Test User", profile.DisplayName);
            Assert.Equal("I love fitness!", profile.Bio);
            Assert.Equal("testuser@fittracker.com", profile.Email);
        }

        [Fact]
        public async Task GetProfileAsync_ReturnsCorrectWorkoutCount()
        {
            var context = TestDbHelper.GetInMemoryDbContext();
            var service = new ProfileService(context);

            var profile = await service.GetProfileAsync("user1");

            Assert.Equal(2, profile.WorkoutCount);
        }

        [Fact]
        public async Task GetProfileAsync_ReturnsCorrectTotalMinutes()
        {
            var context = TestDbHelper.GetInMemoryDbContext();
            var service = new ProfileService(context);

            var profile = await service.GetProfileAsync("user1");

            Assert.Equal(75, profile.TotalMinutes);
        }

        [Fact]
        public async Task GetProfileAsync_ReturnsWorkoutsList()
        {
            var context = TestDbHelper.GetInMemoryDbContext();
            var service = new ProfileService(context);

            var profile = await service.GetProfileAsync("user1");

            Assert.Equal(2, profile.Workouts.Count());
        }

        [Fact]
        public async Task GetProfileForEditAsync_ReturnsCorrectData()
        {
            var context = TestDbHelper.GetInMemoryDbContext();
            var service = new ProfileService(context);

            var model = await service.GetProfileForEditAsync("user1");

            Assert.Equal("Test User", model.DisplayName);
            Assert.Equal("I love fitness!", model.Bio);
        }

        [Fact]
        public async Task GetProfileForEditAsync_ThrowsForInvalidUser()
        {
            var context = TestDbHelper.GetInMemoryDbContext();
            var service = new ProfileService(context);

            await Assert.ThrowsAsync<ArgumentException>(
                () => service.GetProfileForEditAsync("nonexistent"));
        }

        [Fact]
        public async Task UpdateProfileAsync_UpdatesDisplayName()
        {
            var context = TestDbHelper.GetInMemoryDbContext();
            var service = new ProfileService(context);

            var model = new UserProfileEditViewModel
            {
                DisplayName = "Updated Name",
                Bio = "Updated bio"
            };

            await service.UpdateProfileAsync("user1", model);

            var profile = await service.GetProfileAsync("user1");
            Assert.Equal("Updated Name", profile.DisplayName);
            Assert.Equal("Updated bio", profile.Bio);
        }

        [Fact]
        public async Task UpdateProfileAsync_ThrowsForInvalidUser()
        {
            var context = TestDbHelper.GetInMemoryDbContext();
            var service = new ProfileService(context);

            var model = new UserProfileEditViewModel
            {
                DisplayName = "Hacker",
                Bio = "Trying to update nonexistent profile"
            };

            await Assert.ThrowsAsync<ArgumentException>(
                () => service.UpdateProfileAsync("nonexistent", model));
        }

        [Fact]
        public async Task EnsureProfileExistsAsync_CreatesProfileIfNotExists()
        {
            var context = TestDbHelper.GetInMemoryDbContext();
            var service = new ProfileService(context);

            await service.EnsureProfileExistsAsync("user2", "otheruser@fittracker.com");

            var profile = await context.UserProfiles
                .FirstOrDefaultAsync(up => up.UserId == "user2");

            Assert.NotNull(profile);
            Assert.Equal("otheruser", profile.DisplayName);
        }

        [Fact]
        public async Task EnsureProfileExistsAsync_DoesNotDuplicateExistingProfile()
        {
            var context = TestDbHelper.GetInMemoryDbContext();
            var service = new ProfileService(context);

            await service.EnsureProfileExistsAsync("user1", "testuser@fittracker.com");

            var profiles = await context.UserProfiles
                .Where(up => up.UserId == "user1")
                .ToListAsync();

            Assert.Single(profiles);
        }
    }
}
