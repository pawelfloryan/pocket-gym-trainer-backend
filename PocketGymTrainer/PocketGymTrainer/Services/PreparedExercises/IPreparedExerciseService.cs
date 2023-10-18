using PocketGymTrainer.Models;
using ErrorOr;

namespace PocketGymTrainer.Services.PreparedExercises;

public interface IPreparedExerciseService
{
    ErrorOr<List<PreparedExercise>> GetPreparedExerciseList(int position);
    void removeData();
}