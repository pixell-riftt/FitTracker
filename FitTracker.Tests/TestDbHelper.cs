using FitTracker.Data;
using FitTracker.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FitTracker.Tests
{
    public static class TestDbHelper
    {
        public static FitTrackerDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<FitTrackerDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new FitTrackerDbContext(options);

            SeedData(context);

            return context;
        }

        private static void SeedData(FitTrackerDbContext context)
        {
            var user1 = new IdentityUser
            {
                Id = "user1",
                UserName = "testuser@fittracker.com",
                Email = "testuser@fittracker.com"
            };

            var user2 = new IdentityUser
            {
                Id = "user2",
                UserName = "otheruser@fittracker.com",
                Email = "otheruser@fittracker.com"
            };

            context.Users.AddRange(user1, user2);

            context.ExerciseTypes.AddRange(
                new ExerciseType { Id = 1, Name = "Cardio" },
                new ExerciseType { Id = 2, Name = "Strength" },
                new ExerciseType { Id = 3, Name = "Flexibility" }
            );

            context.Workouts.AddRange(
                new Workout
                {
                    Id = 1,
                    Title = "Morning Run",
                    Notes = "Easy 5K run around the park.",
                    DurationMinutes = 30,
                    AuthorId = "user1",
                    CreatedOn = new DateTime(2026, 1, 15),
                    ExerciseTypeId = 1,
                    IsDeleted = false
                },
                new Workout
                {
                    Id = 2,
                    Title = "Bench Press Day",
                    Notes = "Heavy bench press and accessories.",
                    DurationMinutes = 45,
                    AuthorId = "user1",
                    CreatedOn = new DateTime(2026, 1, 20),
                    ExerciseTypeId = 2,
                    IsDeleted = false
                },
                new Workout
                {
                    Id = 3,
                    Title = "Deleted Workout",
                    Notes = "This workout has been deleted.",
                    DurationMinutes = 20,
                    AuthorId = "user1",
                    CreatedOn = new DateTime(2026, 1, 10),
                    ExerciseTypeId = 1,
                    IsDeleted = true
                }
            );

            context.Comments.AddRange(
                new Comment
                {
                    Id = 1,
                    Content = "Great workout!",
                    AuthorId = "user2",
                    WorkoutId = 1,
                    CreatedOn = new DateTime(2026, 1, 16)
                },
                new Comment
                {
                    Id = 2,
                    Content = "Nice routine!",
                    AuthorId = "user1",
                    WorkoutId = 2,
                    CreatedOn = new DateTime(2026, 1, 21)
                }
            );

            context.UserProfiles.Add(
                new UserProfile
                {
                    Id = 1,
                    UserId = "user1",
                    DisplayName = "Test User",
                    Bio = "I love fitness!"
                }
            );

            context.SaveChanges();
        }
    }
}
