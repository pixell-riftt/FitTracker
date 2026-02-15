using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FitTracker.Data.Models;

namespace FitTracker.Data
{
    public class FitTrackerDbContext : IdentityDbContext
    {
        public FitTrackerDbContext(DbContextOptions<FitTrackerDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Workout> Workouts { get; set; } = null!;

        public virtual DbSet<ExerciseType> ExerciseTypes { get; set; } = null!;

        public virtual DbSet<UserWorkout> UsersWorkouts { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Workout>()
                .HasOne(w => w.ExerciseType)
                .WithMany(e => e.Workouts)
                .HasForeignKey(w => w.ExerciseTypeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserWorkout>()
                .HasKey(uw => new { uw.WorkoutId, uw.UserId });

            builder.Entity<UserWorkout>()
                .HasOne(uw => uw.Workout)
                .WithMany(w => w.UsersWorkouts)
                .HasForeignKey(uw => uw.WorkoutId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<UserWorkout>()
                .HasOne(uw => uw.User)
                .WithMany()
                .HasForeignKey(uw => uw.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            var defaultUser = new IdentityUser
            {
                Id = "a1b2c3d4-5678-9012-abcd-ef1234567890",
                UserName = "admin@fittracker.com",
                NormalizedUserName = "ADMIN@FITTRACKER.COM",
                Email = "admin@fittracker.com",
                NormalizedEmail = "ADMIN@FITTRACKER.COM",
                EmailConfirmed = true,
                PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(
                    new IdentityUser { UserName = "admin@fittracker.com" },
                    "Admin123!")
            };
            builder.Entity<IdentityUser>().HasData(defaultUser);

            builder.Entity<ExerciseType>().HasData(
                new ExerciseType { Id = 1, Name = "Cardio" },
                new ExerciseType { Id = 2, Name = "Strength" },
                new ExerciseType { Id = 3, Name = "Flexibility" },
                new ExerciseType { Id = 4, Name = "HIIT" },
                new ExerciseType { Id = 5, Name = "CrossFit" },
                new ExerciseType { Id = 6, Name = "Yoga" }
            );

            // Seed sample workouts
            builder.Entity<Workout>().HasData(
                new Workout
                {
                    Id = 1,
                    Title = "Morning Run",
                    Notes = "Easy 5K run around the park. Keep a steady pace and focus on breathing.",
                    DurationMinutes = 30,
                    ImageUrl = "https://images.unsplash.com/photo-1571008887538-b36bb32f4571?w=500",
                    AuthorId = "a1b2c3d4-5678-9012-abcd-ef1234567890",
                    CreatedOn = new DateTime(2026, 1, 15),
                    ExerciseTypeId = 1,
                    IsDeleted = false
                },
                new Workout
                {
                    Id = 2,
                    Title = "Upper Body Strength",
                    Notes = "Bench press, overhead press, and dumbbell rows. 4 sets of 8 reps each.",
                    DurationMinutes = 45,
                    ImageUrl = "https://images.unsplash.com/photo-1581009146145-b5ef050c2e1e?w=500",
                    AuthorId = "a1b2c3d4-5678-9012-abcd-ef1234567890",
                    CreatedOn = new DateTime(2026, 1, 20),
                    ExerciseTypeId = 2,
                    IsDeleted = false
                },
                new Workout
                {
                    Id = 3,
                    Title = "Yoga Flow Session",
                    Notes = "Full body yoga flow focusing on flexibility and balance. Great for recovery days.",
                    DurationMinutes = 60,
                    ImageUrl = "https://images.unsplash.com/photo-1544367567-0f2fcb009e0b?w=500",
                    AuthorId = "a1b2c3d4-5678-9012-abcd-ef1234567890",
                    CreatedOn = new DateTime(2026, 2, 1),
                    ExerciseTypeId = 6,
                    IsDeleted = false
                }
            );
        }
    }
}