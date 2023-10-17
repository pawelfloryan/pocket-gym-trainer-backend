namespace PocketGymTrainer.Contracts.PreparedExercise;

public record PreparedExerciseResponse(
    Guid id,
    string name,
    string muscleGroup,
    string level,
    string p_p
);