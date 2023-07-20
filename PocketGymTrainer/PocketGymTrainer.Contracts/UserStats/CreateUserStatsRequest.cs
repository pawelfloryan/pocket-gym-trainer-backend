namespace PocketGymTrainer.Contracts.UserStats;

public record CreateUserStatsRequest(
    string id,
    int entries
);