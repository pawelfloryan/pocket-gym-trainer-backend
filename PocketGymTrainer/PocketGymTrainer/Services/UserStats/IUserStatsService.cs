using ErrorOr;
using PocketGymTrainer.Models;

namespace PocketGymTrainer.Services.UserStats;

public interface IUserStatsService
{
    ErrorOr<Created> CreateUserStats(Models.UserStats userStats);
    ErrorOr<Models.UserStats> GetUserStats(string id);
    ErrorOr<UpsertedUserStats> UpsertUserStats(Models.UserStats userStats);
}