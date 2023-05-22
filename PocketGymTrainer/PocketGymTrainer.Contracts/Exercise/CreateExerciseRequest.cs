namespace PocketGymTrainer.Contracts.Exercise;

public record CreateExerciseRequest(
    string sectionId,
    string image,
    string name,
    List<string> description
);