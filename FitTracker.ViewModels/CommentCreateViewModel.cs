using System.ComponentModel.DataAnnotations;
using FitTracker.Common;

namespace FitTracker.ViewModels
{
    public class CommentCreateViewModel
    {
        [Required]
        [StringLength(ValidationConstants.CommentContentMaxLength, MinimumLength = ValidationConstants.CommentContentMinLength)]
        public string Content { get; set; } = null!;

        [Required]
        public int WorkoutId { get; set; }
    }
}