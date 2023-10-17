using PocketGymTrainer.Models;
using ErrorOr;

namespace PocketGymTrainer.Services.PreparedExercises;

public interface IPreparedExerciseService
{
    ErrorOr<List<PreparedExercise>> GetPreparedExerciseList();
    void removeData();
}