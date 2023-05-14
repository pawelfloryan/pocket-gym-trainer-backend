using ErrorOr;
using PocketGymTrainer.Contracts.Exercise;
using PocketGymTrainer.ServiceErrors;
using System.ComponentModel.DataAnnotations;

namespace PocketGymTrainer.Models;

public class Exercise
{
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