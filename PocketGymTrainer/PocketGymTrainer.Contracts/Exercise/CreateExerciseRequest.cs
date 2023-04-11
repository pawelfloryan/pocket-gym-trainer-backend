namespace PocketGymTrainer.Contracts.Exercise;

public record CreateExerciseRequest(
    int sectionId,
    string image,
    string name,
    List<string> description
);