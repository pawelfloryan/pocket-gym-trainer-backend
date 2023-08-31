using PocketGymTrainer.Models;
using ErrorOr;

namespace PocketGymTrainer.Services.Exercises;

public interface IExerciseService
{
    ErrorOr<Created> CreateExercise(Exercise exercise);
    ErrorOr<List<Exercise>> GetExercise(Exercise exercise);
    ErrorOr<UpsertedExercise> UpsertExercise(Exercise exercise);
    ErrorOr<Deleted> DeleteExercise(Guid id, Exercise exercise);
    ErrorOr<Deleted> DeleteExerciseList(Guid id, Exercise exercise);
    Dictionary<Guid, Exercise> addData(Exercise exercise);
    void removeData();
}