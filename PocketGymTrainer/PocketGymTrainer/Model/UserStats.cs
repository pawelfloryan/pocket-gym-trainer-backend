using System.ComponentModel.DataAnnotations;
using ErrorOr;
using PocketGymTrainer.Contracts.UserStats;

namespace PocketGymTrainer.Models;

public class UserStats
{
    [Key]
    public string Id { get; set; }
    public int Entries { get; set; }

    public UserStats(string id, int entries)
    {
        Id = id;
        Entries = entries;
    }

    public static ErrorOr<UserStats> Create(string id, int entries)
    {
        return new UserStats(id, entries);
    }

    public static ErrorOr<UserStats> From(CreateUserStatsRequest request)
    {
        return Create(
            request.id,
            request.entries
        );
    }

    public static ErrorOr<UserStats> From(string id, UpsertUserStatsRequest request)
    {
        return Create(
            id,
            request.entries
        );
    }
}