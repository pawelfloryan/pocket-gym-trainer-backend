using Microsoft.AspNetCore.Mvc;
using PocketGymTrainer.Models;
using PocketGymTrainer.Services.UserStats;
using ErrorOr;
using PocketGymTrainer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using PocketGymTrainer.Contracts.UserStats;

namespace PocketGymTrainer.Controllers;

[ApiController]
//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("userStats")]
public class UserStatsController : ApiController
{
    private readonly IUserStatsService _userStatsService;
    private readonly ApiDbContext _context;

    public UserStatsController(
        IUserStatsService userStatsService, 
        ApiDbContext context
        )
    {
        _userStatsService = userStatsService;
        _context = context;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateUserStats(CreateUserStatsRequest request)
    {
        ErrorOr<UserStats> requestToUserStatsResult = UserStats.From(request);
        
        if (requestToUserStatsResult.IsError)
        {
            return Problem(requestToUserStatsResult.Errors);
        }

        var userStats = requestToUserStatsResult.Value;
        ErrorOr<Created> createdUserStatsResult = _userStatsService.CreateUserStats(userStats);
        
        _context.Add(userStats);
        await _context.SaveChangesAsync();

        _userStatsService.removeData();

        return requestToUserStatsResult.Match(
            created => CreatedAtGetUserStats(userStats),
            errors => Problem(errors)
        );
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetUserStats(string id)
    {
        ErrorOr<UserStats> getUserStatsResult = _userStatsService.GetUserStats(id);

        _userStatsService.removeData();

        return getUserStatsResult.Match(
            userStats => Ok(MapUserStatsResponse(userStats)),
            errors => Problem(errors)
        );
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpsertUserStats(string id, UpsertUserStatsRequest request)
    {
        ErrorOr<UserStats> requestToUserStatsResult = UserStats.From(id, request);

        if(requestToUserStatsResult.IsError)
        {
            return Problem(requestToUserStatsResult.Errors);
        }

        var userStats = requestToUserStatsResult.Value;
        ErrorOr<UpsertedUserStats> upsertUserStatsResult = _userStatsService.UpsertUserStats(userStats);

        _context.Update(userStats);
        await _context.SaveChangesAsync();
        
        _userStatsService.removeData();

        return upsertUserStatsResult.Match(
            upserted => upserted.isNewelyCreated ? CreatedAtGetUserStats(userStats) : NoContent(),
            errors => Problem(errors)
        );
    }

    private static UserStatsResponse MapUserStatsResponse(UserStats userStats)
    {
        return new UserStatsResponse(
            userStats.Id,
            userStats.Entries
        );
    }

    private CreatedAtActionResult CreatedAtGetUserStats(UserStats userStats)
    {
        return CreatedAtAction(
            actionName: nameof(GetUserStats),
            routeValues: new { id = userStats.Id },
            value: MapUserStatsResponse(userStats)
        );
    }
}