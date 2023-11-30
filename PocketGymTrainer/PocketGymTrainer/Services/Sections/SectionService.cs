using ErrorOr;
using PocketGymTrainer.Data;
using PocketGymTrainer.Models;
using PocketGymTrainer.ServiceErrors;
using Microsoft.EntityFrameworkCore;

namespace PocketGymTrainer.Services.Sections;

public class SectionService : ISectionService
{
    private readonly ApiDbContext _context;

    public SectionService(ApiDbContext context)
    {
        _context = context;
    }

    private static readonly Dictionary<Guid, Section> _sections = new();
    private static readonly Dictionary<Guid, Section> _sectionsCreated = new();

    public ErrorOr<Created> CreateSection(Section section)
    {
        addData(section);
        if (_sections.Count > 9)
        {
            removeData();
            return Errors.Section.TooMany;
        }
        else
        {
            _sectionsCreated.Add(section.Id, section);
            return Result.Created;
        }
    }

    public Dictionary<Guid, Section> addData(Section section)
    {
        var allSections = _context.Section.Where(s => s.UserId == section.UserId).ToList();
        foreach (var element in allSections)
        {
            _sections.Add(element.Id, element);
        }
        return _sections;
    }

    public void removeData()
    {
        _sections.Clear();
    }

    public ErrorOr<List<Section>> GetSection(Guid userId)
    {
        List<Section> sectionList = _context.Section.Where(s => s.UserId == userId.ToString()).ToList();

        foreach (var element in sectionList)
        {
            _sections.Add(element.Id, element);
        }

        if (_sections.Count > 0)
        {
            return sectionList;
        }
        return Errors.Section.NotFound;
    }

    public ErrorOr<UpsertedSection> UpsertSection(Section section)
    {
        addData(section);
        var isNewelyCreated = !_sections.ContainsKey(section.Id);
        _sections[section.Id] = section;

        return new UpsertedSection(isNewelyCreated);
    }

    public ErrorOr<Deleted> DeleteSection(Guid id)
    {
        Section? section = _context.Section.FirstOrDefault(s => s.Id == id);
        if (section == null)
        {
            return Errors.Exercise.NotFound;
        }
        addData(section);

        var element = _sections[id];
        _sections.Remove(id);
        _context.Remove(element);

        return Result.Deleted;
    }
}