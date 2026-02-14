using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitTracker.ViewModels
{
    public class WorkoutIndexViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string? ImageUrl { get; set; }

        public string ExerciseType { get; set; } = null!;

        public int DurationMinutes { get; set; }

        public int SavedCount { get; set; }

        public bool IsAuthor { get; set; }

        public bool IsSaved { get; set; }
    }
}
