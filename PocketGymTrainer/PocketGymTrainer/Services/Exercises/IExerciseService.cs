using PocketGymTrainer.Models;
using ErrorOr;

namespace PocketGymTrainer.Services.Exercises;

public interface IExerciseService
{
    ErrorOr<Created> CreateExercise(Exercise exercise);
    ErrorOr<List<Exercise>> GetExercise();
    ErrorOr<UpsertedExercise> UpsertExercise(Exercise exercise);
    ErrorOr<Deleted> DeleteExercise(Guid id);
    ErrorOr<Deleted> DeleteExerciseList(Guid id);
    void removeData();
}