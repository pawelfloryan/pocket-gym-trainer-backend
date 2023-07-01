namespace PocketGymTrainer.Contracts.Workout;

public record CreateWorkoutRequest(
    int time,
    DateTime workoutDate,
    string userId
);