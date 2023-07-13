using ErrorOr;
using PocketGymTrainer.Data;
using PocketGymTrainer.Models;
using PocketGymTrainer.ServiceErrors;

namespace PocketGymTrainer.Services.UserStats;

public class UserStatsService : IUserStatsService
{
    private readonly ApiDbContext _context;

    public UserStatsService(ApiDbContext context)
    {
        _context = context;
    }

    private static readonly Dictionary<string, Models.UserStats> _userStats = new();

    public ErrorOr<Created> CreateUserStats(Models.UserStats userStats)
    {
        _userStats.Add(userStats.Id, userStats);

        return Result.Created;
    }

    public ErrorOr<Models.UserStats> GetUserStats(string id)
    {
        if (_userStats.TryGetValue(id, out var userStats))
        {
            return userStats;
        }
        return Errors.UserStats.NotFound;
    }
}