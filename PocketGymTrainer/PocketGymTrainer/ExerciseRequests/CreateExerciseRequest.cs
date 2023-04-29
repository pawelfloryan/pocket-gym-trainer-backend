namespace PocketGymTrainer.ExerciseRequests;

public record CreateExerciseRequest(
    int sectionId,
    string image,
    string name,
    List<string> description
);