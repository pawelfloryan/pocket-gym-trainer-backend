using ErrorOr;
using PocketGymTrainer.Contracts.Exercise;
using PocketGymTrainer.ServiceErrors;
using System.ComponentModel.DataAnnotations;

namespace PocketGymTrainer.Models;

public class Exercise
{
    [Key]
    public Guid Id { get; set; }
    public string SectionId { get; set; }
    public string UserId { get; set; }
    public string Name { get; set; }
    public bool Completed { get; set; }

    private Exercise()
    {

    }

    private Exercise(
        Guid id,
        string sectionId,
        string userId,
        string name,
        bool completed
        )
    {
        Id = id;
        SectionId = sectionId;
        UserId = userId;
        Name = name;
        Completed = completed;
    }

    public static ErrorOr<Exercise> Create(string sectionId, string userId, string name, bool completed, Guid? id = null)
    {
        return new Exercise(id ?? Guid.NewGuid(), sectionId, userId, name, completed);
    }

    public static ErrorOr<Exercise> From(CreateExerciseRequest request)
    {
        return Create(
            request.sectionId,
            request.userId,
            request.name,
            request.completed
        );
    }

    public static ErrorOr<Exercise> From(Guid id, UpsertExerciseRequest request)
    {
        return Create(
            request.sectionId,
            request.userId,
            request.name,
            request.completed,
            id
        );
    }
}