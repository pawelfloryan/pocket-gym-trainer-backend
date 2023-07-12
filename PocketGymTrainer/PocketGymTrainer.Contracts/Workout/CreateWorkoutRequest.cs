namespace PocketGymTrainer.Contracts.Workout;

public record CreateWorkoutRequest(
    int time,
    string workoutDate,
    string userId
);