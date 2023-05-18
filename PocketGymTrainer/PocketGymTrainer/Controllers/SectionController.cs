using Microsoft.AspNetCore.Mvc;
using PocketGymTrainer.Models;
using PocketGymTrainer.Contracts.Section;
using PocketGymTrainer.Services.Sections;
using ErrorOr;
using PocketGymTrainer.Data;
using Microsoft.EntityFrameworkCore;

namespace PocketGymTrainer.Controllers;

[Route("sections")]
public class SectionController : ApiController
{
    private readonly ISectionService _sectionService;
    private readonly ILogger<SectionController> _logger;
    private readonly ApiDbContext _context;

    public SectionController(
        ISectionService sectionService, 
        ILogger<SectionController> logger,
        ApiDbContext context
        )
    {
        _sectionService = sectionService;
        _logger = logger;
        _context = context;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateSectionAsync(CreateSectionRequest request)
    {
        ErrorOr<Section> requestToSectionResult = Section.From(request);

        if (requestToSectionResult.IsError)
        {
            return Problem(requestToSectionResult.Errors);
        }

        var section = requestToSectionResult.Value;
        ErrorOr<Created> createdSectionResult = _sectionService.CreateSection(section);
        
        _context.Add(section);
        await _context.SaveChangesAsync();
        
        _sectionService.removeData();

        return requestToSectionResult.Match(
            created => CreatedAtGetSection(section),
            errors => Problem(errors)
        );
    }

    [HttpGet]
    public async Task<IActionResult> GetSection()
    {
        ErrorOr<List<Section>> getSectionResult = _sectionService.GetSection();
        var allSections = await _context.Section.ToListAsync();

        _sectionService.removeData();

        return getSectionResult.Match(
            allSections => Ok(MapSectionListResponseAsync(allSections)),
            errors => Problem(errors)
        );
    }

    [HttpPut("{id:guid}")]
    public IActionResult UpsertSection(Guid id, UpsertSectionRequest request)
    {
        ErrorOr<Section> requestToSectionResult = Section.From(id, request);

        if(requestToSectionResult.IsError)
        {
            return Problem(requestToSectionResult.Errors);
        }

        var section = requestToSectionResult.Value;
        ErrorOr<UpsertedSection> upsertSectionResult = _sectionService.UpsertSection(section);

        return upsertSectionResult.Match(
            upserted => upserted.isNewelyCreated ? CreatedAtGetSection(section) : NoContent(),
            errors => Problem(errors)
        );
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteSection(Guid id)
    {
        ErrorOr<Deleted> deleteSectionResult = _sectionService.DeleteSection(id);
        //var section = deleteSectionResult.Value;
//
        //_context.Remove(id);
        await _context.SaveChangesAsync();
        
        _sectionService.removeData();

        return deleteSectionResult.Match(
            deleted => NoContent(),
            errors => Problem(errors)
        );
    }

    private static SectionResponse MapSectionResponse(Section section)
    {
        return new SectionResponse(
            section.Id,
            section.Name
        );
    }

    private static List<SectionResponse> MapSectionListResponseAsync(List<Section> sectionList)
    {
        List<SectionResponse> responseList = new();
        foreach(Section section in sectionList)
        {
            SectionResponse response = new SectionResponse(
                section.Id,
                section.Name
            );
            responseList.Add(response);
        }
        return responseList;
    }

    private CreatedAtActionResult CreatedAtGetSection(Section section)
    {
        return CreatedAtAction(
            actionName: nameof(GetSection),
            routeValues: new { id = section.Id },
            value: MapSectionResponse(section)
        );
    }
}