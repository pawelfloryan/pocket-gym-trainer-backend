namespace PocketGymTrainer.Contracts.Exercise;

public record UpsertExerciseRequest(
    int sectionId,
    string image,
    string name,
    List<string> description
);