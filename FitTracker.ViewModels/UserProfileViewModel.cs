using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitTracker.ViewModels
{
    public class UserProfileViewModel
    {
        public string UserId { get; set; } = null!;

        public string DisplayName { get; set; } = null!;

        public string? Bio { get; set; }

        public string? ProfileImageUrl { get; set; }

        public string Email { get; set; } = null!;

        public int WorkoutCount { get; set; }

        public int TotalMinutes { get; set; }

        public IEnumerable<WorkoutIndexViewModel> Workouts { get; set; } = new List<WorkoutIndexViewModel>();
    }
}
