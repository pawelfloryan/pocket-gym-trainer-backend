using ErrorOr;
using PocketGymTrainer.Data;
using PocketGymTrainer.Models;
using PocketGymTrainer.ServiceErrors;

namespace PocketGymTrainer.Services.PreparedExercises;

public class PreparedExerciseService : IPreparedExerciseService
{
    private readonly ApiDbContext _context;

    public PreparedExerciseService(ApiDbContext context)
    {
        _context = context;
    }

    private static readonly Dictionary<Guid, PreparedExercise> _preparedExercises = new();
    const int recordTake = 25;

    public void removeData()
    {
        _preparedExercises.Clear();
    }

    public ErrorOr<List<PreparedExercise>> GetPreparedExerciseList(int position)
    {
        List<PreparedExercise> preparedExerciseList = _context.PreparedExercise.OrderBy(e => e.Id).Skip(position).Take(recordTake).ToList();

        foreach (var element in preparedExerciseList)
        {
            _preparedExercises.Add(element.Id, element);
        }

        if (_preparedExercises.Count > 0)
        {
            return preparedExerciseList;
        }
        return Errors.PreparedExercise.NotFound;
    }
}