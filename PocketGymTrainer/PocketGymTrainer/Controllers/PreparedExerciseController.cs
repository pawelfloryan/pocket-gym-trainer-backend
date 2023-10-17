using Microsoft.AspNetCore.Mvc;
using PocketGymTrainer.Models;
using PocketGymTrainer.Contracts.Exercise;
using PocketGymTrainer.Services.Exercises;
using ErrorOr;
using PocketGymTrainer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace PocketGymTrainer.Controllers;

[ApiController]
//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("preparedExercises")]
public class PreparedExerciseExerciseController : ApiController
{
    private readonly IPreparedExerciseService _preparedExerciseService;
    private readonly ApiDbContext _context;

    public PreparedExerciseController(IPreparedExerciseService preparedExerciseService,
        ApiDbContext context
        )
    {
        _preparedExerciseService = preparedExerciseService;
        _context = context;
    }

    [HttpGet]
    public IActionResult GetExercise(Guid id)
    {
        ErrorOr<List<Exercise>> getExerciseResult = _preparedExerciseService.GetExercise(id);

        List<Exercise> exercises = getExerciseResult.Value;
        _preparedExerciseService.removeData();

        return getExerciseResult.Match(
            exercise => Ok(MapExerciseListResponseAsync(exercises)),
            errors => Problem(errors)
        );
    }

    private static ExerciseResponse MapExerciseResponse(Exercise exercise)
    {
        return new ExerciseResponse(
            exercise.Id,
            exercise.SectionId,
            exercise.UserId,
            exercise.Name
        );
    }

    private static List<ExerciseResponse> MapExerciseListResponseAsync(List<PreparedExercise> preparedExerciseList)
    {
        List<ExerciseResponse> responseList = new();
        foreach (PreparedExercise preparedExercise in preparedExerciseList)
        {
            ExerciseResponse response = new ExerciseResponse(
                preparedExercise.Id,
                preparedExercise.SectionId,
                preparedExercise.UserId,
                preparedExercise.Name
            );
            responseList.Add(response);
        }
        return responseList;
    }

    private CreatedAtActionResult CreatedAtGetExercise(Exercise exercise)
    {
        return CreatedAtAction(
            actionName: nameof(GetExercise),
            routeValues: new { id = exercise.Id },
            value: MapExerciseResponse(exercise)
        );
    }
}