namespace PocketGymTrainer.Contracts.Exercise;

public record UpsertExerciseRequest(
    int exerciseId,
    string image,
    string name,
    List<string> description
);