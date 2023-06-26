using PocketGymTrainer.Models;
using ErrorOr;

namespace PocketGymTrainer.Services.Workouts;

public interface IWorkoutService
{
    ErrorOr<Created> CreateWorkout(Workout workout);
    ErrorOr<List<Workout>> GetWorkout();
    void removeData();
}