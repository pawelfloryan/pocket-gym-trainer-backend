namespace PocketGymTrainer.Contracts.Section;

public record CreateSectionRequest(
    string name,
    string userId,
    int exercisesPerformed
);