using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitTracker.ViewModels
{
    public class WorkoutDetailsViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string? ImageUrl { get; set; }

        public string Notes { get; set; } = null!;

        public int DurationMinutes { get; set; }

        public string ExerciseType { get; set; } = null!;

        public string Author { get; set; } = null!;

        public DateTime CreatedOn { get; set; }

        public bool IsAuthor { get; set; }

        public bool IsSaved { get; set; }
    }
}
