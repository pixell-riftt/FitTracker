using System.ComponentModel.DataAnnotations;
using FitTracker.Common;

namespace FitTracker.ViewModels
{
    public class WorkoutEditViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(ValidationConstants.WorkoutTitleMaxLength, MinimumLength = ValidationConstants.WorkoutTitleMinLength)]
        public string Title { get; set; } = null!;

        [Required]
        [StringLength(ValidationConstants.WorkoutNotesMaxLength, MinimumLength = ValidationConstants.WorkoutNotesMinLength)]
        public string Notes { get; set; } = null!;

        [Required]
        [Range(ValidationConstants.WorkoutDurationMinValue, ValidationConstants.WorkoutDurationMaxValue)]
        public int DurationMinutes { get; set; }

        public string? ImageUrl { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public string CreatedOn { get; set; } = null!;

        [Required]
        [Range(ValidationConstants.ExerciseTypeIdMinValue, ValidationConstants.ExerciseTypeIdMaxValue)]
        public int ExerciseTypeId { get; set; }

        public IEnumerable<ExerciseTypeViewModel>? ExerciseTypes { get; set; }
    }
}