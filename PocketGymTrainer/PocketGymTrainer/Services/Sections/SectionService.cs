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
    public ErrorOr<Created> CreateSection(Section section)
    {
        addGetData();
        _sections.Add(section.Id, section);

        return Result.Created;
    }
    
    public Dictionary<Guid, Section> addGetData()
    {
        var allSections = _context.Section.ToList();
        foreach(var element in allSections)
        {
            _sections.Add(element.Id, element);
        }
        return _sections;
    }

    public ErrorOr<List<Section>> GetSection()
    {
        addGetData();
        List<Section> sectionList = _sections.Values.ToList();
        if(_sections.Count > 0)
        {
            return sectionList;
        }
        return Errors.Section.NotFound;
    }

    public ErrorOr<UpsertedSection> UpsertSection(Section section)
    {
        //addGetData();
        var isNewelyCreated = !_sections.ContainsKey(section.Id);
        _sections[section.Id] = section;

        return new UpsertedSection(isNewelyCreated);
    }

    public ErrorOr<Deleted> DeleteSection(Guid id)
    {
        //addGetData();
        _sections.Remove(id);

        return Result.Deleted;
    }
}