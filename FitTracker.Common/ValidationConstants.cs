using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitTracker.Common
{
    public class ValidationConstants
    {
        // Workout
        public const int WorkoutTitleMinLength = 3;
        public const int WorkoutTitleMaxLength = 80;

        public const int WorkoutNotesMinLength = 10;
        public const int WorkoutNotesMaxLength = 1000;

        public const int WorkoutDurationMinValue = 1;
        public const int WorkoutDurationMaxValue = 600;

        // ExerciseType
        public const int ExerciseTypeNameMinLength = 3;
        public const int ExerciseTypeNameMaxLength = 30;
        public const int ExerciseTypeIdMinValue = 1;
        public const int ExerciseTypeIdMaxValue = 6;

        // Comment
        public const int CommentContentMinLength = 2;
        public const int CommentContentMaxLength = 500;

        // UserProfile
        public const int DisplayNameMinLength = 2;
        public const int DisplayNameMaxLength = 50;

        public const int BioMaxLength = 500;
    }
}
