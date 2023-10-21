using Microsoft.AspNetCore.Mvc;
using PocketGymTrainer.Models;
using PocketGymTrainer.Contracts.Exercise;
using PocketGymTrainer.Services.PreparedExercises;
using ErrorOr;
using PocketGymTrainer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using PocketGymTrainer.Contracts.PreparedExercise;

namespace PocketGymTrainer.Controllers;

[ApiController]
//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("preparedExercises")]
public class PreparedExerciseController : ApiController
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
    public async Task<IActionResult> GetExercise([FromQuery(Name = "position")] int position = 0)
    {
        _preparedExerciseService.removeData();
        ErrorOr<List<PreparedExercise>> getPreparedExerciseResult = await _preparedExerciseService.GetPreparedExerciseList(position);

        List<PreparedExercise> preparedExercises = getPreparedExerciseResult.Value;
        _preparedExerciseService.removeData();

        return getPreparedExerciseResult.Match(
            preparedExercise => Ok(MapPreparedExerciseListResponse(preparedExercises)),
            errors => Problem(errors)
        );
    }

    private static List<PreparedExerciseResponse> MapPreparedExerciseListResponse(List<PreparedExercise> preparedExerciseList)
    {
        List<PreparedExerciseResponse> responseList = new();
        foreach (PreparedExercise preparedExercise in preparedExerciseList)
        {
            PreparedExerciseResponse response = new PreparedExerciseResponse(
                preparedExercise.Id,
                preparedExercise.Name,
                preparedExercise.MuscleGroup,
                preparedExercise.Level,
                preparedExercise.P_P
            );
            responseList.Add(response);
        }
        return responseList;
    }
}