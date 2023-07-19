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
    private static readonly Dictionary<string, Models.UserStats> _userStatsCreated = new();

    public ErrorOr<Created> CreateUserStats(Models.UserStats userStats)
    {
        _userStatsCreated.Add(userStats.Id, userStats);

        return Result.Created;
    }

    public Dictionary<string, Models.UserStats> addGetData()
    {
        var allUserStats = _context.UserStats.ToList();
        foreach(var element in allUserStats)
        {
            _userStats.Add(element.Id, element);
        }
        return _userStats;
    }

    public void removeData()
    {
        _userStats.Clear();
    }

    public ErrorOr<Models.UserStats> GetUserStats(string id)
    {
        addGetData();
        if (_userStats.TryGetValue(id, out var userStats))
        {
            return userStats;
        }
        return Errors.UserStats.NotFound;
    }

    public ErrorOr<UpsertedUserStats> UpsertUserStats(Models.UserStats userStats)
    {
        var isNewelyCreated = !_userStats.ContainsKey(userStats.Id);
        _userStats[userStats.Id] = userStats;

        return new UpsertedUserStats(isNewelyCreated);
    }
}