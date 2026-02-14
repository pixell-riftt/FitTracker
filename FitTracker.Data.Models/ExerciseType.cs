using System.ComponentModel.DataAnnotations;
using FitTracker.Common;

namespace FitTracker.Data.Models
{
    public class ExerciseType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(ValidationConstants.ExerciseTypeNameMaxLength)]
        public string Name { get; set; } = null!;

        public virtual ICollection<Workout> Workouts { get; set; } = new HashSet<Workout>();
    }
}