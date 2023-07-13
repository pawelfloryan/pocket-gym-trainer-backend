namespace PocketGymTrainer.Contracts.User;

public record UserResponse(
    Guid Id,
    int entries
);