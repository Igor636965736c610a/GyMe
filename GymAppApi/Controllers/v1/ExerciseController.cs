using GymAppApi.BodyRequest.Exercise;
using GymAppApi.Routes.v1;
using GymAppCore.Models.Entities;
using GymAppInfrastructure.Dtos.Exercise;
using GymAppInfrastructure.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace GymAppApi.Controllers.v1;

[Authorize]
[Route("[controller]")]
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
    public async Task<IActionResult> Get([FromRoute] string id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var exerciseId = Guid.Parse(id);
        var userId = Guid.Parse(UtilsControllers.GetUserIdFromClaim(HttpContext));
        
        var exercise = await _exerciseService.GetExercise(userId, exerciseId);
        
        return Ok(exercise);
    }

    [HttpGet(ApiRoutes.Exercise.GetAll)]
    public async Task<IActionResult> GetAll([FromQuery] string userId,[FromQuery] int page,[FromQuery] int size)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var jwtId = Guid.Parse(UtilsControllers.GetUserIdFromClaim(HttpContext));
        var id =  Guid.Parse(userId);

        var exercises = await _exerciseService.GetExercises(jwtId, id, page, size);

        return Ok(exercises);
    }

    [HttpPut(ApiRoutes.Exercise.Update)]
    public async Task<IActionResult> Update([FromBody] PutExerciseBody putExerciseBody,[FromRoute] string id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var userId = Guid.Parse(UtilsControllers.GetUserIdFromClaim(HttpContext));
        var exerciseId = Guid.Parse(id);
        
        PutExerciseDto putExerciseDto = new()
        {
            Position = putExerciseBody.Position
        };
        await _exerciseService.UpdateExercise(userId, exerciseId, putExerciseDto);
        
        return Ok();
    }

    [HttpDelete(ApiRoutes.Exercise.Remove)]
    public async Task<IActionResult> Remove([FromRoute] string id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var userId = Guid.Parse(UtilsControllers.GetUserIdFromClaim(HttpContext));
        var exerciseId = Guid.Parse(id);
        
        await _exerciseService.RemoveExercise(userId, exerciseId);
        
        return Ok();
    }
}