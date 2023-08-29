using PocketGymTrainer.Models;
using ErrorOr;

namespace PocketGymTrainer.Services.Exercises;

public interface IExerciseService
{
    ErrorOr<Created> CreateExercise(Exercise exercise);
    ErrorOr<List<Exercise>> GetExercise(Guid userId);
    ErrorOr<UpsertedExercise> UpsertExercise(Exercise exercise);
    ErrorOr<Deleted> DeleteExercise(Guid id);
    ErrorOr<Deleted> DeleteExerciseList(Guid id);
    Dictionary<Guid, Exercise> addData(Exercise exercise);
    void removeData();
}