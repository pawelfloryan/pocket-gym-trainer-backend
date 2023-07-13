using ErrorOr;
using PocketGymTrainer.Models;

namespace PocketGymTrainer.Services.Users;

public interface IUserService
{
    ErrorOr<Created> CreateUser(User user);
    ErrorOr<User> GetUser(Guid id);
}