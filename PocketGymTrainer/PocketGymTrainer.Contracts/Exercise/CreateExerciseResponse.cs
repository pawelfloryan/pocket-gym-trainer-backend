namespace PocketGymTrainer.Contracts.Exercise;

public record CreateExerciseResponse(
    Guid id,
    int sectionId,
    string image,
    string name,
    List<string> description
);