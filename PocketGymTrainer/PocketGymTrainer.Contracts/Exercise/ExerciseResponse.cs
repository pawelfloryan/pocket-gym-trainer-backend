namespace PocketGymTrainer.Contracts.Exercise;

public record ExerciseResponse(
    Guid id,
    string sectionId,
    string name
);