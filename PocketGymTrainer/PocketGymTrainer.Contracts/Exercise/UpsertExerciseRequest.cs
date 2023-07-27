namespace PocketGymTrainer.Contracts.Exercise;

public record UpsertExerciseRequest(
    string sectionId,
    string userId,
    string name,
    bool completed
);