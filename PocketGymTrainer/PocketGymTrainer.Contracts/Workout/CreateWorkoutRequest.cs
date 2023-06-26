namespace PocketGymTrainer.Contracts.Workout;

public record CreateWorkoutRequest(
    int time,
    string weekDay,
    string userId
);