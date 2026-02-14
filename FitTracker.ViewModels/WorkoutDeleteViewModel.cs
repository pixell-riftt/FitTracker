using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitTracker.ViewModels
{
    public class WorkoutDeleteViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string AuthorId { get; set; } = null!;

        public string Author { get; set; } = null!;
    }
}
