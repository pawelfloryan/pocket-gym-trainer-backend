using ErrorOr;
using PocketGymTrainer.Contracts.Section;
using PocketGymTrainer.ServiceErrors;
using System.ComponentModel.DataAnnotations;

namespace PocketGymTrainer.Models;

public class Section
{
    public const int MaxNameLength = 8;
    public const int MinNameLength = 3;
    [Key]
    public Guid Id { get; }
    public string Name { get; }

    private Section()
    {

    }
    
    private Section(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    public static ErrorOr<Section> Create(string name, Guid? id = null)
    {
        List<Error> errors = new();

        if(name.Length is < MinNameLength or > MaxNameLength)
        {
            errors.Add(Errors.Section.InvalidName);
        }

        if(errors.Count > 0)
        {
            return errors;
        }

        return new Section(id ?? Guid.NewGuid(), name);
    }

    public static ErrorOr<Section> From(CreateSectionRequest request)
    {
        return Create(
            request.name
        );
    }

    public static ErrorOr<Section> From(Guid id, UpsertSectionRequest request)
    {
        return Create(
            request.name,
            id
        );
    }
}