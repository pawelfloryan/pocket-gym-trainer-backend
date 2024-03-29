using Microsoft.AspNetCore.Mvc;
using PocketGymTrainer.Models;
using PocketGymTrainer.Contracts.Section;
using PocketGymTrainer.Services.Photos;
using ErrorOr;
using PocketGymTrainer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace PocketGymTrainer.Controllers;

[ApiController]
//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("static")]
public class ImageController : ApiController
{
    private readonly IStorageProvider _storageProvider;
    private readonly ApiDbContext _context;

    public ImageController(
        IStorageProvider storageProvider, 
        ApiDbContext context
        )
    {
        _storageProvider = storageProvider;
        _context = context;
    }

    //private static SectionResponse MapSectionResponse(Section section)
    //{
    //    return new SectionResponse(
    //        section.Id,
    //        section.Name,
    //        section.UserId
    //    );
    //}
//
    //private static List<SectionResponse> MapSectionListResponseAsync(List<Section> sectionList)
    //{
    //    List<SectionResponse> responseList = new();
    //    foreach(Section section in sectionList)
    //    {
    //        SectionResponse response = new SectionResponse(
    //            section.Id,
    //            section.Name,
    //            section.UserId
    //        );
    //        responseList.Add(response);
    //    }
    //    return responseList;
    //}

    //private CreatedAtActionResult CreatedAtGetSection(Section section)
    //{
    //    return CreatedAtAction(
    //        actionName: nameof(GetSection),
    //        routeValues: new { id = section.Id },
    //        value: MapSectionResponse(section)
    //    );
    //}
}