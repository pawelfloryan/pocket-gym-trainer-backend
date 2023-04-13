using ErrorOr;
using PocketGymTrainer.Contracts.Exercise;
using PocketGymTrainer.ServiceErrors;

namespace PocketGymTrainer.Models;

public class Exercise
{
    public const int MinNameLength = 3;
    public Guid Id { get; }
    public int SectionId { get; }
    public string Image { get; }
    public string Name { get; }
    public List<string> Description { get; }

    private Exercise(
        Guid id, 
        int sectionId, 
        string image, 
        string name, 
        List<string> description
        )
    {
        Id = id;
        SectionId = sectionId;
        Image = image;
        Name = name;
        Description = description;
    }

    public static ErrorOr<Exercise> Create(int sectionId, string image, string name, List<string> description, Guid? id = null)
    {
        List<Error> errors = new();

        if(name.Length is < MinNameLength)
        {
            errors.Add(Errors.Exercise.InvalidName);
        }

        if(errors.Count > 0)
        {
            return errors;
        }

        return new Exercise(id ?? Guid.NewGuid(), sectionId, image, name, description);
    }
    
    public static ErrorOr<Exercise> From(CreateExerciseRequest request)
    {
        return Create(
            request.sectionId,
            request.image,
            request.name,
            request.description
        );
    }

    public static ErrorOr<Exercise> From(Guid id, UpsertExerciseRequest request)
    {
        return Create(
            request.sectionId,
            request.image,
            request.name,
            request.description,
            id
        );
    }
}