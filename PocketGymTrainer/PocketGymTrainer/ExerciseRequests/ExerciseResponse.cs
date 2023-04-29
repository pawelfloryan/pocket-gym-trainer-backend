namespace PocketGymTrainer.ExerciseRequests;

public record ExerciseResponse(
    Guid id,
    int sectionId,
    string image,
    string name,
    List<string> description
);