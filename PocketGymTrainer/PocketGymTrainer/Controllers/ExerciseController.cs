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
[Route("exercises")]
public class ExerciseController : ApiController
{
    private readonly IExerciseService _exerciseService;
    private readonly ApiDbContext _context;

    public ExerciseController(IExerciseService exerciseService,
        ApiDbContext context
        )
    {
        _exerciseService = exerciseService;
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> CreateExercise(CreateExerciseRequest request)
    {
        ErrorOr<Exercise> requestToExerciseResult = Exercise.From(request);

        if (requestToExerciseResult.IsError)
        {
            return Problem(requestToExerciseResult.Errors);
        }


        var exercise = requestToExerciseResult.Value;
        ErrorOr<Created> createdExerciseResult = _exerciseService.CreateExercise(exercise);

        if (createdExerciseResult.IsError)
        {
            return Problem(createdExerciseResult.Errors);
        }

        _context.Add(exercise);
        await _context.SaveChangesAsync();

        _exerciseService.removeData();

        return requestToExerciseResult.Match(
            created => CreatedAtGetExercise(exercise),
            errors => Problem(errors)
        );
    }

    [HttpGet("{id:guid}/{sectionId}")]
    public IActionResult GetExercise(Guid id, string sectionId)
    {
        ErrorOr<List<Exercise>> getExerciseResult = _exerciseService.GetExercise(id, sectionId);

        List<Exercise> exercises = getExerciseResult.Value;
        _exerciseService.removeData();

        return getExerciseResult.Match(
            exercise => Ok(MapExerciseListResponseAsync(exercises)),
            errors => Problem(errors)
        );
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpsertExercise(Guid id, UpsertExerciseRequest request)
    {
        ErrorOr<Exercise> requestToExerciseResult = Exercise.From(id, request);

        if (requestToExerciseResult.IsError)
        {
            return Problem(requestToExerciseResult.Errors);
        }

        var exercise = requestToExerciseResult.Value;
        ErrorOr<UpsertedExercise> upsertExerciseResult = _exerciseService.UpsertExercise(exercise);

        var existingExercise = _context.Exercise.Find(exercise.Id);

        if (existingExercise == null)
        {
            _context.Add(exercise);
            await _context.SaveChangesAsync();

            _exerciseService.removeData();
        }
        else
        {
            _context.Entry(existingExercise).CurrentValues.SetValues(exercise);
            await _context.SaveChangesAsync();

            _exerciseService.removeData();
        }


        return upsertExerciseResult.Match(
            upserted => upserted.isNewelyCreated ? CreatedAtGetExercise(exercise) : NoContent(),
            errors => Problem(errors)
        );
    }

    [HttpDelete("single/{id:guid}")]
    public async Task<IActionResult> DeleteExercise(Guid id)
    {
        ErrorOr<Deleted> deleteExerciseResult = _exerciseService.DeleteExercise(id);
        await _context.SaveChangesAsync();

        _exerciseService.removeData();

        return deleteExerciseResult.Match(
            deleted => NoContent(),
            errors => Problem(errors)
        );
    }

    [HttpDelete("list/{id:guid}")]
    public async Task<IActionResult> DeleteExerciseList(Guid id)
    {
        ErrorOr<Deleted> deleteExerciseListResult = _exerciseService.DeleteExerciseList(id);
        await _context.SaveChangesAsync();

        _exerciseService.removeData();

        return deleteExerciseListResult.Match(
            deleted => NoContent(),
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

    private static List<ExerciseResponse> MapExerciseListResponseAsync(List<Exercise> exerciseList)
    {
        List<ExerciseResponse> responseList = new();
        foreach (Exercise exercise in exerciseList)
        {
            ExerciseResponse response = new ExerciseResponse(
                exercise.Id,
                exercise.SectionId,
                exercise.UserId,
                exercise.Name
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