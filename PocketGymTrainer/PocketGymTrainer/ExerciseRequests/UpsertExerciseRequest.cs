namespace PocketGymTrainer.ExerciseRequests;

public record UpsertExerciseRequest(
    int sectionId,
    string image,
    string name,
    List<string> description
);