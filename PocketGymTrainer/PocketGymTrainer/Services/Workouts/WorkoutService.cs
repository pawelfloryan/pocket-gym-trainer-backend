using ErrorOr;
using PocketGymTrainer.Data;
using PocketGymTrainer.Models;
using PocketGymTrainer.ServiceErrors;
using Microsoft.EntityFrameworkCore;

namespace PocketGymTrainer.Services.Workouts;

public class WorkoutService : IWorkoutService
{
    private readonly ApiDbContext _context;

    public WorkoutService(ApiDbContext context)
    {
        _context = context;
    }

    private static readonly Dictionary<Guid, Workout> _workouts = new();
    private static readonly Dictionary<Guid, Workout> _workoutsCreated = new();

    public ErrorOr<Created> CreateWorkout(Workout workout)
    {
        _workoutsCreated.Add(workout.Id, workout);

        return Result.Created;
    }

    public Dictionary<Guid, Workout> addGetData()
    {
        var allWorkouts = _context.Workout.ToList();
        foreach (var element in allWorkouts)
        {
            _workouts.Add(element.Id, element);
        }
        return _workouts;
    }

    public void removeData()
    {
        _workouts.Clear();
    }

    public ErrorOr<List<Workout>> GetWorkout()
    {
        addGetData();
        List<Workout> workoutList = _workouts.Values.ToList();
        if (_workouts.Count > 0)
        {
            return workoutList;
        }
        return Errors.Workout.NotFound;
    }
}