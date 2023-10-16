using ErrorOr;
using PocketGymTrainer.Contracts.Exercise;
using PocketGymTrainer.ServiceErrors;
using System.ComponentModel.DataAnnotations;

namespace PocketGymTrainer.Models;

public class PrepredExercise
{
    [Key]
    public Guid Id { get; set; }
    public string SectionId { get; set; }
    public string UserId { get; set; }
    public string Name { get; set; }

    private PrepredExercise()
    {

    }

    private PreparedExercise(
        Guid id,
        string sectionId,
        string userId,
        string name
        )
    {
        Id = id;
        SectionId = sectionId;
        UserId = userId;
        Name = name;
    }

    public static ErrorOr<PrepredExercise> Create(string sectionId, string userId, string name, Guid? id = null)
    {
        return new PrepredExercise(id ?? Guid.NewGuid(), sectionId, userId, name);
    }

    public static ErrorOr<PrepredExercise> From(CreatePrepredExerciseRequest request)
    {
        return Create(
            request.sectionId,
            request.userId,
            request.name
        );
    }
}