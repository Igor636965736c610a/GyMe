using GymAppApi.BodyRequest.Exercise;
using GymAppApi.Routes.v1;
using GymAppCore.Models.Entities;
using GymAppInfrastructure.Dtos.Exercise;
using GymAppInfrastructure.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymAppApi.Controllers.v1;

[Authorize]
public class ExerciseController : ControllerBase
{
    private readonly IExerciseService _exerciseService;
    private readonly IUserService _userService;
    public ExerciseController(IExerciseService exerciseService, IUserService userService)
    {
        _exerciseService = exerciseService;
        _userService = userService;
    }

    [HttpPost(ApiRoutes.Exercise.Create)]
    public async Task<IActionResult> CreateExercise([FromBody] PostExerciseBody postExerciseBody)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _userService.Get(HttpContext.User);
        if (user is null)
            throw new Exception("na pozniej");

        PostExerciseDto postExerciseDto = new()
        {
            ExercisesType = postExerciseBody.ExercisesType,
            Position = postExerciseBody.Position
        };

        await _exerciseService.CreateExercise(postExerciseDto, user);

        return Ok();
    }
}