namespace PocketGymTrainer.Contracts.Exercise;

public record CreateExerciseRequest(
    int exerciseId,
    string image,
    string name,
    List<string> description
);