using ErrorOr;
using PocketGymTrainer.Data;
using PocketGymTrainer.Models;
using PocketGymTrainer.ServiceErrors;

namespace PocketGymTrainer.Services.Exercises;

public class ExerciseService : IExerciseService
{
    private readonly ApiDbContext _context;

    public ExerciseService(ApiDbContext context)
    {
        _context = context;
    }

    private static readonly Dictionary<Guid, Exercise> _exercises = new();
    private static readonly Dictionary<Guid, Exercise> _exercisesCreated = new();
    public ErrorOr<Created> CreateExercise(Exercise exercise)
    {
        _exercisesCreated.Add(exercise.Id, exercise);

        return Result.Created;
    }

    public Dictionary<Guid, Exercise> addGetData()
    {
        var allExercises = _context.Exercise.ToList();
        foreach(var element in allExercises)
        {
            _exercises.Add(element.Id, element);
        }
        return _exercises;
    }

    public void removeData()
    {
        _exercises.Clear();
    }

    public ErrorOr<List<Exercise>> GetExercise()
    {
        addGetData();
        List<Exercise> exerciseList = _exercises.Values.ToList(); 
        if(_exercises.Count > 0)
        {
            return exerciseList;
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
        addGetData();
        var exercise = _exercises[id];
        _exercises.Remove(id);
        _context.Remove(exercise);

        return Result.Deleted;
    }
}