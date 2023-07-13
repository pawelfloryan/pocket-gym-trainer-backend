using Microsoft.AspNetCore.Mvc;
using PocketGymTrainer.Models;
using PocketGymTrainer.Contracts.User;
using PocketGymTrainer.Services.Users;
using ErrorOr;
using PocketGymTrainer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace PocketGymTrainer.Controllers;

[ApiController]
//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("users")]
public class UserController : ApiController
{
    private readonly IUserService _userService;
    private readonly ApiDbContext _context;

    public UserController(
        IUserService userService, 
        ApiDbContext context
        )
    {
        _userService = userService;
        _context = context;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserRequest request)
    {
        ErrorOr<User> requestToUserResult = User.From(request);
        
        if (requestToUserResult.IsError)
        {
            return Problem(requestToUserResult.Errors);
        }

        var user = requestToUserResult.Value;
        ErrorOr<Created> createdUserResult = _userService.CreateUser(user);
        
        _context.Add(user);
        await _context.SaveChangesAsync();

        return requestToUserResult.Match(
            created => CreatedAtGetUser(user),
            errors => Problem(errors)
        );
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetUser(Guid id)
    {
        ErrorOr<User> getUserResult = _userService.GetUser(id);

        return getUserResult.Match(
            user => Ok(MapUserResponse(user)),
            errors => Problem(errors)
        );
    }

    private static UserResponse MapUserResponse(User user)
    {
        return new UserResponse(
            user.Id,
            user.Entries
        );
    }

    private CreatedAtActionResult CreatedAtGetUser(User user)
    {
        return CreatedAtAction(
            actionName: nameof(GetUser),
            routeValues: new { id = user.Id },
            value: MapUserResponse(user)
        );
    }
}