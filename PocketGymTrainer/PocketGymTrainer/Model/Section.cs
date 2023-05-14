using ErrorOr;
using PocketGymTrainer.Contracts.Section;
using PocketGymTrainer.ServiceErrors;
using System.ComponentModel.DataAnnotations;

namespace PocketGymTrainer.Models;

public class Section
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; }

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
        return new Section(id ?? Guid.NewGuid(), name);
    }

    public static Section CreateS(string name, Guid? id = null)
    {
        List<Error> errors = new();

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