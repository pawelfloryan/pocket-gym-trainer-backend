using System.ComponentModel.DataAnnotations;
using ErrorOr;

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

    public static ErrorOr<UserStats> From(UserStats request)
    {
        return Create(
            request.Id,
            request.Entries
        );
    }
}