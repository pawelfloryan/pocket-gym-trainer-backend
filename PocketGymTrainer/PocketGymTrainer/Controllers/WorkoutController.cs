using Microsoft.AspNetCore.Mvc;
using PocketGymTrainer.Models;
using PocketGymTrainer.Contracts.Workout;
using PocketGymTrainer.Services.Workouts;
using ErrorOr;
using PocketGymTrainer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace PocketGymTrainer.Controllers;

[ApiController]
//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("workouts")]
public class WorkoutController : ApiController
{
    private readonly IWorkoutService _workoutService;
    private readonly ApiDbContext _context;

    public WorkoutController(
        IWorkoutService workoutService, 
        ApiDbContext context
        )
    {
        _workoutService = workoutService;
        _context = context;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateWorkout(CreateWorkoutRequest request)
    {
        ErrorOr<Workout> requestToWorkoutResult = Workout.From(request);

        if (requestToWorkoutResult.IsError)
        {
            return Problem(requestToWorkoutResult.Errors);
        }

        var workout = requestToWorkoutResult.Value;
        ErrorOr<Created> createdWorkoutResult = _workoutService.CreateWorkout(workout);
        
        _context.Add(workout);
        await _context.SaveChangesAsync();

        _workoutService.removeData();

        return requestToWorkoutResult.Match(
            created => CreatedAtGetWorkout(workout),
            errors => Problem(errors)
        );
    }

    [HttpGet]
    public async Task<IActionResult> GetWorkout()
    {
        ErrorOr<List<Workout>> getWorkoutResult = _workoutService.GetWorkout();
        var allWorkouts = await _context.Workout.ToListAsync();

        _workoutService.removeData();

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

    private CreatedAtActionResult CreatedAtGetWorkout(Workout workout)
    {
        return CreatedAtAction(
            actionName: nameof(GetWorkout),
            routeValues: new { id = workout.Id },
            value: MapWorkoutResponse(workout)
        );
    }
}