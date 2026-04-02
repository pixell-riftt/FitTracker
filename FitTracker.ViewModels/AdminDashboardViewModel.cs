using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitTracker.ViewModels
{
    public class AdminDashboardViewModel
    {
        public int TotalUsers { get; set; }

        public int TotalWorkouts { get; set; }

        public int TotalComments { get; set; }

        public int TotalExerciseTypes { get; set; }
    }
}
