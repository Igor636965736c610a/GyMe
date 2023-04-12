using GymAppApi.BodyRequest.Exercise;
using GymAppApi.Routes.v1;
using GymAppCore.Models.Entities;
using GymAppInfrastructure.Dtos.Exercise;
using GymAppInfrastructure.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymAppApi.Controllers.v1;

[Authorize]
public class ExerciseController : ControllerBase
{
    private readonly IExerciseService _exerciseService;
    public ExerciseController(IExerciseService exerciseService)
    {
        _exerciseService = exerciseService;
    }

    [HttpPost(ApiRoutes.Exercise.Create)]
    public async Task<IActionResult> CreateExercise([FromBody] PostExerciseBody postExerciseBody)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = Guid.Parse(UtilsControllers.GetUserIdFromClaim(HttpContext));

        PostExerciseDto postExerciseDto = new()
        {
            ExercisesType = postExerciseBody.ExercisesType,
            Position = postExerciseBody.Position
        };

        await _exerciseService.CreateExercise(postExerciseDto, userId);

        return Ok();
    }

    [HttpGet(ApiRoutes.Exercise.Get)]
    public async Task<IActionResult> Get([FromQuery] ExercisesType exercisesType)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
            
        var userId = Guid.Parse(UtilsControllers.GetUserIdFromClaim(HttpContext));
        var exercise = await _exerciseService.GetExercise(userId, exercisesType);
        
        return Ok(exercise);
    }

    [HttpGet(ApiRoutes.Exercise.GetAll)]
    public async Task<IActionResult> GetAll()
    {
        var userId = Guid.Parse(UtilsControllers.GetUserIdFromClaim(HttpContext));
        var exercises = await _exerciseService.GetExercises(userId);
        
        return Ok(exercises);
    }

    [HttpPut(ApiRoutes.Exercise.Update)]
    public async Task<IActionResult> Update([FromBody] PutExerciseBody putExerciseBody, [FromQuery] ExercisesType exercisesType)
    {
        var userId = Guid.Parse(UtilsControllers.GetUserIdFromClaim(HttpContext));
        PutExerciseDto putExerciseDto = new()
        {
            Position = putExerciseBody.Position
        };
        await _exerciseService.UpdateExercise(userId, exercisesType, putExerciseDto);
        
        return Ok();
    }

    [HttpDelete(ApiRoutes.Exercise.Remove)]
    public async Task<IActionResult> Remove([FromQuery] ExercisesType exercisesType)
    {
        var userId = Guid.Parse(UtilsControllers.GetUserIdFromClaim(HttpContext));
        await _exerciseService.RemoveExercise(userId, exercisesType);
        
        return Ok();
    }
}