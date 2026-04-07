using FitTracker.Data;
using FitTracker.Services.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace FitTracker.Tests
{
    public class AdminServiceTests
    {
        private static UserManager<IdentityUser> GetMockUserManager(FitTrackerDbContext context)
        {
            var store = new Mock<IUserStore<IdentityUser>>();
            var mgr = new Mock<UserManager<IdentityUser>>(
                store.Object, null, null, null, null, null, null, null, null);

            mgr.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((string id) => context.Users.FirstOrDefault(u => u.Id == id));

            mgr.Setup(x => x.GetRolesAsync(It.IsAny<IdentityUser>()))
                .ReturnsAsync(new List<string>());

            mgr.Setup(x => x.AddToRoleAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            mgr.Setup(x => x.RemoveFromRoleAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            return mgr.Object;
        }

        [Fact]
        public async Task GetDashboardAsync_ReturnsCorrectCounts()
        {
            var context = TestDbHelper.GetInMemoryDbContext();
            var userManager = GetMockUserManager(context);
            var service = new AdminService(context, userManager);

            var dashboard = await service.GetDashboardAsync();

            Assert.Equal(2, dashboard.TotalUsers);
            Assert.Equal(2, dashboard.TotalWorkouts);
            Assert.Equal(2, dashboard.TotalComments);
            Assert.Equal(3, dashboard.TotalExerciseTypes);
        }

        [Fact]
        public async Task GetAllUsersAsync_ReturnsAllUsers()
        {
            var context = TestDbHelper.GetInMemoryDbContext();
            var userManager = GetMockUserManager(context);
            var service = new AdminService(context, userManager);

            var users = await service.GetAllUsersAsync();

            Assert.Equal(2, users.Count());
        }

        [Fact]
        public async Task GetAllUsersAsync_ReturnsCorrectWorkoutCount()
        {
            var context = TestDbHelper.GetInMemoryDbContext();
            var userManager = GetMockUserManager(context);
            var service = new AdminService(context, userManager);

            var users = (await service.GetAllUsersAsync()).ToList();
            var user1 = users.First(u => u.Id == "user1");

            Assert.Equal(2, user1.WorkoutCount);
        }

        [Fact]
        public async Task DeleteWorkoutAsAdminAsync_SoftDeletesWorkout()
        {
            var context = TestDbHelper.GetInMemoryDbContext();
            var userManager = GetMockUserManager(context);
            var service = new AdminService(context, userManager);

            await service.DeleteWorkoutAsAdminAsync(1);

            var workout = await context.Workouts.FindAsync(1);
            Assert.True(workout.IsDeleted);
        }

        [Fact]
        public async Task DeleteWorkoutAsAdminAsync_DoesNothingForInvalidId()
        {
            var context = TestDbHelper.GetInMemoryDbContext();
            var userManager = GetMockUserManager(context);
            var service = new AdminService(context, userManager);

            await service.DeleteWorkoutAsAdminAsync(999);

            var activeWorkouts = await context.Workouts.Where(w => !w.IsDeleted).CountAsync();
            Assert.Equal(2, activeWorkouts);
        }

        [Fact]
        public async Task DeleteCommentAsAdminAsync_DeletesComment()
        {
            var context = TestDbHelper.GetInMemoryDbContext();
            var userManager = GetMockUserManager(context);
            var service = new AdminService(context, userManager);

            await service.DeleteCommentAsAdminAsync(1);

            var comment = await context.Comments.FindAsync(1);
            Assert.Null(comment);
        }

        [Fact]
        public async Task DeleteCommentAsAdminAsync_DoesNothingForInvalidId()
        {
            var context = TestDbHelper.GetInMemoryDbContext();
            var userManager = GetMockUserManager(context);
            var service = new AdminService(context, userManager);

            await service.DeleteCommentAsAdminAsync(999);

            var commentCount = await context.Comments.CountAsync();
            Assert.Equal(2, commentCount);
        }

        [Fact]
        public async Task PromoteToAdminAsync_CallsAddToRole()
        {
            var context = TestDbHelper.GetInMemoryDbContext();
            var store = new Mock<IUserStore<IdentityUser>>();
            var mgr = new Mock<UserManager<IdentityUser>>(
                store.Object, null, null, null, null, null, null, null, null);

            mgr.Setup(x => x.FindByIdAsync("user1"))
                .ReturnsAsync(context.Users.First(u => u.Id == "user1"));
            mgr.Setup(x => x.AddToRoleAsync(It.IsAny<IdentityUser>(), "Admin"))
                .ReturnsAsync(IdentityResult.Success);

            var service = new AdminService(context, mgr.Object);

            await service.PromoteToAdminAsync("user1");

            mgr.Verify(x => x.AddToRoleAsync(It.IsAny<IdentityUser>(), "Admin"), Times.Once);
        }

        [Fact]
        public async Task DemoteFromAdminAsync_CallsRemoveFromRole()
        {
            var context = TestDbHelper.GetInMemoryDbContext();
            var store = new Mock<IUserStore<IdentityUser>>();
            var mgr = new Mock<UserManager<IdentityUser>>(
                store.Object, null, null, null, null, null, null, null, null);

            mgr.Setup(x => x.FindByIdAsync("user1"))
                .ReturnsAsync(context.Users.First(u => u.Id == "user1"));
            mgr.Setup(x => x.RemoveFromRoleAsync(It.IsAny<IdentityUser>(), "Admin"))
                .ReturnsAsync(IdentityResult.Success);

            var service = new AdminService(context, mgr.Object);

            await service.DemoteFromAdminAsync("user1");

            mgr.Verify(x => x.RemoveFromRoleAsync(It.IsAny<IdentityUser>(), "Admin"), Times.Once);
        }
    }
}
