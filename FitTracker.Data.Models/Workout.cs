using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using FitTracker.Common;

namespace FitTracker.Data.Models
{
    public class Workout
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(ValidationConstants.WorkoutTitleMaxLength)]
        public string Title { get; set; } = null!;

        [Required]
        [MaxLength(ValidationConstants.WorkoutNotesMaxLength)]
        public string Notes { get; set; } = null!;

        [Required]
        public int DurationMinutes { get; set; }

        public string? ImageUrl { get; set; }

        [Required]
        public string AuthorId { get; set; } = null!;
        public virtual IdentityUser Author { get; set; } = null!;

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public int ExerciseTypeId { get; set; }
        public virtual ExerciseType ExerciseType { get; set; } = null!;

        public bool IsDeleted { get; set; } = false;

        public virtual ICollection<UserWorkout> UsersWorkouts { get; set; } = new HashSet<UserWorkout>();

        public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
    }
}