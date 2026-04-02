using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitTracker.ViewModels
{
    public class AdminUserViewModel
    {
        public string Id { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string? DisplayName { get; set; }

        public bool IsAdmin { get; set; }

        public int WorkoutCount { get; set; }
    }
}
