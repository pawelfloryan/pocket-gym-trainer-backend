namespace PocketGymTrainer.Contracts.Trainer;

public record CreateActivityRequest(
    string image,
    string name,
    List<string> description
);