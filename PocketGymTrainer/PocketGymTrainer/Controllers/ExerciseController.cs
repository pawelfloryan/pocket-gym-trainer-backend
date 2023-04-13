using Microsoft.AspNetCore.Mvc;
using PocketGymTrainer.Models;
using PocketGymTrainer.Contracts.Exercise;
using PocketGymTrainer.Services.Exercises;
using ErrorOr;

namespace PocketGymTrainer.Controllers;

[Route("exercises")]
public class ExerciseController : ApiController
{
    private readonly IExerciseService _exerciseService;

    public ExerciseController(IExerciseService exerciseService)
    {
        _exerciseService = exerciseService;
    }
    
    [HttpPost]
    public IActionResult CreateExercise(CreateExerciseRequest request)
    {
        ErrorOr<Exercise> requestToExerciseResult = Exercise.From(request);

        if (requestToExerciseResult.IsError)
        {
            return Problem(requestToExerciseResult.Errors);
        }

        var exercise = requestToExerciseResult.Value;
        ErrorOr<Created> createdExerciseResult = _exerciseService.CreateExercise(exercise);

        return requestToExerciseResult.Match(
            created => CreatedAtGetExercise(exercise),
            errors => Problem(errors)
        );
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetExercise(Guid id)
    {
        ErrorOr<Exercise> getExerciseResult = _exerciseService.GetExercise(id);

        return getExerciseResult.Match(
            exercise => Ok(MapExerciseResponse(exercise)),
            errors => Problem(errors)
        );
    }

    [HttpPut("{id:guid}")]
    public IActionResult UpsertExercise(Guid id, UpsertExerciseRequest request)
    {
        ErrorOr<Exercise> requestToExerciseResult = Exercise.From(id, request);

        if(requestToExerciseResult.IsError)
        {
            return Problem(requestToExerciseResult.Errors);
        }

        var exercise = requestToExerciseResult.Value;
        ErrorOr<UpsertedExercise> upsertExerciseResult = _exerciseService.UpsertExercise(exercise);

        return upsertExerciseResult.Match(
            upserted => upserted.isNewelyCreated ? CreatedAtGetExercise(exercise) : NoContent(),
            errors => Problem(errors)
        );
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteExercise(Guid id)
    {
        ErrorOr<Deleted> deleteExerciseResult = _exerciseService.DeleteExercise(id);

        return deleteExerciseResult.Match(
            deleted => NoContent(),
            errors => Problem(errors)
        );
    }

    private static ExerciseResponse MapExerciseResponse(Exercise exercise)
    {
        return new ExerciseResponse(
            exercise.Id,
            exercise.SectionId,
            exercise.Image,
            exercise.Name,
            exercise.Description
        );

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