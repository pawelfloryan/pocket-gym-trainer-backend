using ErrorOr;
using PocketGymTrainer.Models;
using PocketGymTrainer.ServiceErrors;

namespace PocketGymTrainer.Services.Exercises;

public class ExerciseService : IExerciseService
{
    private static readonly Dictionary<Guid, Exercise> _exercises = new();
    public ErrorOr<Created> CreateExercise(Exercise exercise)
    {
        _exercises.Add(exercise.Id, exercise);

        return Result.Created;
    }

    public ErrorOr<Exercise> GetExercise(Guid id)
    {
        if(_exercises.TryGetValue(id, out var exercise))
        {
            return exercise;
        }
        return Errors.Exercise.NotFound;
    }

    public ErrorOr<UpsertedExercise> UpsertExercise(Exercise exercise)
    {
        var isNewelyCreated = !_exercises.ContainsKey(exercise.Id);
        _exercises[exercise.Id] = exercise;

        return new UpsertedExercise(isNewelyCreated);
    }

    public ErrorOr<Deleted> DeleteExercise(Guid id)
    {
        _exercises.Remove(id);

        return Result.Deleted;
    }
}