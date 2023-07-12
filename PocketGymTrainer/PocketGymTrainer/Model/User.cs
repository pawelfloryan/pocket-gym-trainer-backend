using System.ComponentModel.DataAnnotations;
using ErrorOr;
using PocketGymTrainer.Contracts.User;

namespace PocketGymTrainer.Models;

public class User
{
    [Key]
    public Guid Id { get; set; }
    public int Entries { get; set; }

    private User(Guid id, int entries)
    {
        Id = id;
        Entries = entries;
    }

    public static ErrorOr<User> Create(int entries, Guid? id = null)
    {
        return new User(id ?? Guid.NewGuid(), entries);
    }

    public static ErrorOr<User> From(CreateUserRequest request)
    {
        return Create(
            request.entries
        );
    }
}