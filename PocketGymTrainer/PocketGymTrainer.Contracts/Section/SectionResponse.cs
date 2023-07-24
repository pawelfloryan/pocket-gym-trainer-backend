namespace PocketGymTrainer.Contracts.Section;

public record SectionResponse(
    Guid Id,
    string name,
    string userId,
    int exercisesPerformed
);