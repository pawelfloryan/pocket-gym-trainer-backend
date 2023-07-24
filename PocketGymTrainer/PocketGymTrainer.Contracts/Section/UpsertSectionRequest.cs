namespace PocketGymTrainer.Contracts.Section;

public record UpsertSectionRequest(
    string name,
    string userId,
    int exercisesPerformed
);