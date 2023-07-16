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
    public string UserId { get; set; }

    private Section()
    {

    }
    
    private Section(Guid id, string name, string userId)
    {
        Id = id;
        Name = name;
        UserId = userId;
    }

    public static ErrorOr<Section> Create(string name, string userId, Guid? id = null)
    {
        return new Section(id ?? Guid.NewGuid(), name, userId);
    }

    public static ErrorOr<Section> From(CreateSectionRequest request)
    {
        return Create(
            request.name,
            request.userId
        );
    }

    public static ErrorOr<Section> From(Guid id, UpsertSectionRequest request)
    {
        return Create(
            request.name,
            request.userId,
            id
        );
    }
}