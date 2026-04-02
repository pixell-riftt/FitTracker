using System.ComponentModel.DataAnnotations;
using FitTracker.Common;

namespace FitTracker.ViewModels
{
    public class UserProfileEditViewModel
    {
        [Required]
        [StringLength(ValidationConstants.DisplayNameMaxLength, MinimumLength = ValidationConstants.DisplayNameMinLength)]
        public string DisplayName { get; set; } = null!;

        [MaxLength(ValidationConstants.BioMaxLength)]
        public string? Bio { get; set; }

        public string? ProfileImageUrl { get; set; }
    }
}