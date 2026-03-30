using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using FitTracker.Common;

namespace FitTracker.Data.Models
{
    public class UserProfile
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = null!;
        public virtual IdentityUser User { get; set; } = null!;

        [Required]
        [MaxLength(ValidationConstants.DisplayNameMaxLength)]
        public string DisplayName { get; set; } = null!;

        [MaxLength(ValidationConstants.BioMaxLength)]
        public string? Bio { get; set; }

        public string? ProfileImageUrl { get; set; }
    }
}