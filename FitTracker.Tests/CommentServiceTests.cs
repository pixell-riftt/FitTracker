using FitTracker.Services.Core;
using FitTracker.ViewModels;
using Moq;
using Xunit;

namespace FitTracker.Tests
{
    public class CommentServiceTests
    {
        [Fact]
        public async Task GetCommentsForWorkoutAsync_ReturnsCorrectComments()
        {
            var context = TestDbHelper.GetInMemoryDbContext();
            var service = new WorkoutService(context);

            var comments = await service.GetCommentsForWorkoutAsync(1);
            Assert.Single(comments);
            Assert.Equal("Great workout!", comments.First().Content);
        }

        [Fact]
        public async Task GetCommentsForWorkoutAsync_ReturnsEmptyForNoComments()
        {
            var context = TestDbHelper.GetInMemoryDbContext();
            var service = new WorkoutService(context);

            var comments = await service.GetCommentsForWorkoutAsync(999);
            Assert.Empty(comments);
        }

        [Fact]
        public async Task AddCommentAsync_AddsCommentToWorkout()
        {
            var context = TestDbHelper.GetInMemoryDbContext();
            var service = new WorkoutService(context);

            var model = new CommentCreateViewModel
            {
                Content = "This is a new comment!",
                WorkoutId = 1
            };

            await service.AddCommentAsync(model, "user2");

            var comments = await service.GetCommentsForWorkoutAsync(1);
            Assert.Equal(2, comments.Count());
        }

        [Fact]
        public async Task AddCommentAsync_SetsCorrectAuthor()
        {
            var context = TestDbHelper.GetInMemoryDbContext();
            var service = new WorkoutService(context);

            var model = new CommentCreateViewModel
            {
                Content = "Comment by user1",
                WorkoutId = 2
            };

            await service.AddCommentAsync(model, "user1");

            var comments = await service.GetCommentsForWorkoutAsync(2);
            var newComment = comments.First(c => c.Content == "Comment by user1");
            Assert.Equal("user1", newComment.AuthorId);
        }

        [Fact]
        public async Task DeleteCommentAsync_DeletesOwnComment()
        {
            var context = TestDbHelper.GetInMemoryDbContext();
            var service = new WorkoutService(context);

            await service.DeleteCommentAsync(2, "user1", false);

            var comments = await service.GetCommentsForWorkoutAsync(2);
            Assert.Empty(comments);
        }

        [Fact]
        public async Task DeleteCommentAsync_AdminCanDeleteAnyComment()
        {
            var context = TestDbHelper.GetInMemoryDbContext();
            var service = new WorkoutService(context);

            await service.DeleteCommentAsync(1, "user1", true);

            var comments = await service.GetCommentsForWorkoutAsync(1);
            Assert.Empty(comments);
        }

        [Fact]
        public async Task DeleteCommentAsync_ThrowsForNonAuthorNonAdmin()
        {
            var context = TestDbHelper.GetInMemoryDbContext();
            var service = new WorkoutService(context);

            await Assert.ThrowsAsync<UnauthorizedAccessException>(
                () => service.DeleteCommentAsync(1, "user1", false));
        }

        [Fact]
        public async Task DeleteCommentAsync_ThrowsForInvalidId()
        {
            var context = TestDbHelper.GetInMemoryDbContext();
            var service = new WorkoutService(context);

            await Assert.ThrowsAsync<ArgumentException>(
                () => service.DeleteCommentAsync(999, "user1", false));
        }

        [Fact]
        public async Task GetCommentsForWorkoutAsync_OrdersByNewestFirst()
        {
            var context = TestDbHelper.GetInMemoryDbContext();
            var service = new WorkoutService(context);

            var model1 = new CommentCreateViewModel { Content = "First comment added", WorkoutId = 1 };
            var model2 = new CommentCreateViewModel { Content = "Second comment added", WorkoutId = 1 };

            await service.AddCommentAsync(model1, "user1");
            await service.AddCommentAsync(model2, "user2");

            var comments = (await service.GetCommentsForWorkoutAsync(1)).ToList();
            Assert.True(comments[0].CreatedOn >= comments[1].CreatedOn);
        }
    }
}
