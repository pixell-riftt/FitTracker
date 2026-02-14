using Microsoft.AspNetCore.Identity;

namespace FitTracker.Data.Models
{
    public class UserWorkout
    {
        public string UserId { get; set; } = null!;
        public virtual IdentityUser User { get; set; } = null!;

        public int WorkoutId { get; set; }
        public virtual Workout Workout { get; set; } = null!;
    }
}