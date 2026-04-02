using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitTracker.ViewModels
{
    public class CommentViewModel
    {
        public int Id { get; set; }

        public string Content { get; set; } = null!;

        public string AuthorName { get; set; } = null!;

        public string AuthorId { get; set; } = null!;

        public DateTime CreatedOn { get; set; }
    }
}
