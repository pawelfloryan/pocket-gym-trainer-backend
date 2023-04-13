using PocketGymTrainer.Models;
using ErrorOr;

namespace PocketGymTrainer.Services.Exercises;

public interface IExerciseService
{
    ErrorOr<Created> CreateExercise(Exercise exercise);
    ErrorOr<Exercise> GetExercise(Guid id);
    ErrorOr<UpsertedExercise> UpsertExercise(Exercise exercise);
    ErrorOr<Deleted> DeleteExercise(Guid id);
}