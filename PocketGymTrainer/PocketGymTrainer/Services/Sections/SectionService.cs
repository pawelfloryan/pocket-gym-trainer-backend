using ErrorOr;
using PocketGymTrainer.Models;
using PocketGymTrainer.ServiceErrors;

namespace PocketGymTrainer.Services.Sections;

public class SectionService : ISectionService
{
    private static readonly Dictionary<Guid, Section> _sections = new();
    public ErrorOr<Created> CreateSection(Section section)
    {
        _sections.Add(section.Id, section);

        return Result.Created;
    }

    public ErrorOr<List<Section>> GetSection(Guid id)
    {
        List<Section> sectionList = new();
        if(_sections.Any())
        {
            foreach(KeyValuePair<Guid, Section> element in _sections)
            {
                if(_sections.TryGetValue(id, out var section))
                {
                    sectionList.Add(section);
                }
            }
            return sectionList;
        }
        return Errors.Section.NotFound;
    }

    public ErrorOr<UpsertedSection> UpsertSection(Section section)
    {
        var isNewelyCreated = !_sections.ContainsKey(section.Id);
        _sections[section.Id] = section;

        return new UpsertedSection(isNewelyCreated);
    }

    public ErrorOr<Deleted> DeleteSection(Guid id)
    {
        _sections.Remove(id);

        return Result.Deleted;
    }
}