namespace PocketGymTrainer.Contracts.Exercise;

public record UpsertExerciseRequest(
    string sectionId,
    string image,
    string name,
    List<string> description
);