namespace PocketGymTrainer.Contracts.Exercise;

public record CreateExerciseRequest(
    string sectionId,
    string userId,
    string name,
    bool completed
);