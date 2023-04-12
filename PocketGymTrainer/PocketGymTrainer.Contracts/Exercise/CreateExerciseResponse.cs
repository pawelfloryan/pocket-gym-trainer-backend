namespace PocketGymTrainer.Contracts.Exercise;

public record CreateExerciseResponse(
    Guid id,
    int exerciseId,
    string image,
    string name,
    List<string> description
);