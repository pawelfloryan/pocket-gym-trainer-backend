using Microsoft.AspNetCore.Mvc;
using PocketGymTrainer.Models;
using PocketGymTrainer.Contracts.Section;
using PocketGymTrainer.Services.Sections;
using ErrorOr;

namespace PocketGymTrainer.Controllers;

public class SectionController : ApiController
{
    private readonly ISectionService _sectionService;

    public SectionController(ISectionService sectionService)
    {
        _sectionService = sectionService;
    }
    
    [HttpPost]
    public IActionResult CreateSection(CreateSectionRequest request)
    {
        ErrorOr<Section> requestToSectionResult = Section.From(request);

        if (requestToSectionResult.IsError)
        {
            return Problem(requestToSectionResult.Errors);
        }

        var section = requestToSectionResult.Value;
        ErrorOr<Created> createdSectionResult = _sectionService.CreateSection(section);

        return requestToSectionResult.Match(
            created => CreatedAtGetSection(section),
            errors => Problem(errors)
        );
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetSection(Guid id)
    {
        ErrorOr<Section> getSectionResult = _sectionService.GetSection(id);

        return getSectionResult.Match(
            section => Ok(MapSectionResponse(section)),
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
    public IActionResult DeleteSection(Guid id)
    {
        ErrorOr<Deleted> deleteSectionResult = _sectionService.DeleteSection(id);

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

    private CreatedAtActionResult CreatedAtGetSection(Section section)
    {
        return CreatedAtAction(
            actionName: nameof(GetSection),
            routeValues: new { id = section.Id },
            value: MapSectionResponse(section)
        );
    }
}