using FitTracker.ViewModels;

namespace FitTracker.Services.Core.Contracts
{
    public interface IWorkoutService
    {
        Task<IEnumerable<WorkoutIndexViewModel>> GetAllWorkoutsAsync(string? userId);

        Task<WorkoutIndexViewModel?> GetWorkoutByIdAsync(int id);

        Task<WorkoutDetailsViewModel> GetWorkoutDetailsByIdAsync(int id);

        Task<bool> IsWorkoutSavedAsync(int workoutId, string userId);

        Task<bool> IsWorkoutAuthorAsync(int workoutId, string userId);

        Task<WorkoutCreateViewModel> GetWorkoutCreateViewModelAsync();

        Task AddWorkoutAsync(WorkoutCreateViewModel model, string authorId);

        Task<IEnumerable<WorkoutFavoritesViewModel>> GetFavoriteWorkoutsAsync(string userId);

        Task SaveWorkoutAsync(int id, string userId);

        Task RemoveWorkoutAsync(int id, string userId);

        Task<WorkoutEditViewModel> GetWorkoutForEditAsync(int id, string userId);

        Task EditWorkoutAsync(WorkoutEditViewModel model, string userId);

        Task<IEnumerable<ExerciseTypeViewModel>> GetAllExerciseTypesAsync();

        Task DeleteWorkoutAsync(int id, string userId);

        Task<WorkoutDeleteViewModel> GetWorkoutDeleteDetailsAsync(int id, string userId);

        Task<IEnumerable<WorkoutIndexViewModel>> GetMyWorkoutsAsync(string userId);

        Task<IEnumerable<CommentViewModel>> GetCommentsForWorkoutAsync(int workoutId);

        Task AddCommentAsync(CommentCreateViewModel model, string userId);

        Task DeleteCommentAsync(int commentId, string userId, bool isAdmin);
    }
}
