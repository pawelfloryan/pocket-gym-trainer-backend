namespace PocketGymTrainer.Contracts.Workout;

public record WorkoutResponse(
    Guid Id,
    int time,
    DateTime workoutDate,
    string userId
);