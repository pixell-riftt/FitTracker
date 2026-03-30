using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using FitTracker.Common;

namespace FitTracker.Data.Models
{
    public class Comment
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(ValidationConstants.CommentContentMaxLength)]
        public string Content { get; set; } = null!;

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public string AuthorId { get; set; } = null!;
        public virtual IdentityUser Author { get; set; } = null!;

        [Required]
        public int WorkoutId { get; set; }
        public virtual Workout Workout { get; set; } = null!;
    }
}