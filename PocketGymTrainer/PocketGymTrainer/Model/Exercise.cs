using ErrorOr;
using PocketGymTrainer.ExerciseRequests;
using PocketGymTrainer.ServiceErrors;
using System.ComponentModel.DataAnnotations;

namespace PocketGymTrainer.Models;

public class Exercise
{
    public const int MinNameLength = 3;
    [Key]
    public Guid Id { get; set; }
    public int SectionId { get; set; }
    public string Image { get; set; }
    public string Name { get; set; }
    public List<string> Description { get; set; }

    private Exercise()
    {

    }
    
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