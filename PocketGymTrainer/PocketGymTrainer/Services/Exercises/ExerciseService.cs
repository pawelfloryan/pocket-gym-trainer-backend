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
        addData(exercise);
        if (_exercises.Count > 14)
        {
            removeData();
            return Errors.Exercise.TooMany;
        }
        else
        {
            _exercisesCreated.Add(exercise.Id, exercise);
            return Result.Created;
        }
    }

    public Dictionary<Guid, Exercise> addData(Exercise exercise)
    {
        var allExercises = _context.Exercise.Where(e => e.UserId == exercise.UserId).ToList();
        foreach (var element in allExercises)
        {
            _exercises.Add(element.Id, element);
        }
        return _exercises;
    }

    public void removeData()
    {
        _exercises.Clear();
    }

    public ErrorOr<List<Exercise>> GetExercise(Exercise exercise)
    {
        List<Exercise> exerciseList = _context.Exercise.Where(e => e.UserId == exercise.UserId).ToList();
        if (_exercises.Count > 0)
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

    public ErrorOr<Deleted> DeleteExercise(Guid id, Exercise exercise)
    {
        addData(exercise);
        var element = _exercises[id];
        _exercises.Remove(id);
        _context.Remove(element);

        return Result.Deleted;
    }

    public ErrorOr<Deleted> DeleteExerciseList(Guid id, Exercise exercise)
    {
        addData(exercise);
        var exerciseList = _exercises.Values.Where(exercise => exercise.SectionId == id.ToString()).ToList();
        for (int i = 0; i < exerciseList.Count; i++)
        {
            _exercises.Remove(exerciseList[i].Id);
        }
        foreach (Exercise element in exerciseList)
        {
            _context.Remove(element);
        }

        return Result.Deleted;
    }
}