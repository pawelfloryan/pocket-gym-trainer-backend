namespace PocketGymTrainer.Contracts.Exercise;

public record UpsertExerciseRequest(
    string sectionId,
    string name
);