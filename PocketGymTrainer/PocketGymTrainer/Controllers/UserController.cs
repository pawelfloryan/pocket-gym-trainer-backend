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

        _userService.removeData();

        return requestToUserResult.Match(
            created => CreatedAtGetUser(user),
            errors => Problem(errors)
        );
    }

    [HttpGet]
    public async Task<IActionResult> GetUser()
    {
        ErrorOr<List<Workout>> getWorkoutResult = _userService.GetWorkout();
        var allWorkouts = await _context.Workout.ToListAsync();

        _userService.removeData();

        return getWorkoutResult.Match(
            allWorkouts => Ok(MapWorkoutListResponseAsync(allWorkouts)),
            errors => Problem(errors)
        );
    }

    private static WorkoutResponse MapWorkoutResponse(Workout workout)
    {
        return new WorkoutResponse(
            workout.Id,
            workout.Time,
            workout.WorkoutDate,
            workout.UserId
        );
    }

    private static List<WorkoutResponse> MapWorkoutListResponseAsync(List<Workout> workoutList)
    {
        List<WorkoutResponse> responseList = new();
        foreach(Workout workout in workoutList)
        {
            WorkoutResponse response = new WorkoutResponse(
                workout.Id,
                workout.Time,
                workout.WorkoutDate,
                workout.UserId
            );
            responseList.Add(response);
        }
        return responseList;
    }

    private CreatedAtActionResult CreatedAtGetUser(User user)
    {
        return CreatedAtAction(
            actionName: nameof(GetUser),
            routeValues: new { id = workout.Id },
            value: MapWorkoutResponse(workout)
        );
    }
}