using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitTracker.ViewModels
{
    public class WorkoutFavoritesViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string ExerciseType { get; set; } = null!;

        public string? ImageUrl { get; set; }
    }
}
