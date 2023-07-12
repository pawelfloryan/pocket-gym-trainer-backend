namespace PocketGymTrainer.Contracts.Workout;

public record WorkoutResponse(
    Guid Id,
    int time,
    string workoutDate,
    string userId
);