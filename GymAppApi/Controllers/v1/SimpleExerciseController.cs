using GymAppApi.BodyRequest.SimpleExercise;
using GymAppApi.Routes.v1;
using GymAppInfrastructure.Dtos.SimpleExercise;
using GymAppInfrastructure.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymAppApi.Controllers.v1;

[Authorize]
[Route("[controller]")]
public class SimpleExerciseController : ControllerBase
{
    private readonly ISimpleExerciseService _simpleExerciseService;
    public SimpleExerciseController(ISimpleExerciseService simpleExerciseService)
    {
        _simpleExerciseService = simpleExerciseService;
    }

    [HttpPost(ApiRoutes.SimpleExercise.Create)]
    public async Task<IActionResult> CreateSimpleExercise([FromBody] PostSimpleExerciseBody postSimpleExerciseBody)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
            
        var userId = Guid.Parse(UtilsControllers.GetUserIdFromClaim(HttpContext));

        PostSimpleExerciseDto postSimpleExerciseDto = new()
        {
            ExerciseId = Guid.Parse(postSimpleExerciseBody.ExerciseId),
            Series = postSimpleExerciseBody.Series,
            Description = postSimpleExerciseBody.Description
        };

        await _simpleExerciseService.CreateSimpleExercise(postSimpleExerciseDto, userId);

        return Ok();
    }

    [HttpPut(ApiRoutes.SimpleExercise.Update)]
    public async Task<IActionResult> UpdateSimpleExercise([FromBody] PutSimpleExerciseBody putSimpleExerciseBody)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
            
        var userId = Guid.Parse(UtilsControllers.GetUserIdFromClaim(HttpContext));
        var exerciseId = Guid.Parse(putSimpleExerciseBody.ExerciseId);

        PutSimpleExerciseDto putSimpleExerciseDto = new()
        {
            Series = putSimpleExerciseBody.Series,
            Description = putSimpleExerciseBody.Description
        };

        await _simpleExerciseService.UpdateSimpleExercise(userId, exerciseId, putSimpleExerciseDto);
        
        return Ok();
    }

    [HttpGet(ApiRoutes.SimpleExercise.Get)]
    public async Task<IActionResult> GetSimpleExercise([FromQuery] string exerciseId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
            
        var userId = Guid.Parse(UtilsControllers.GetUserIdFromClaim(HttpContext));
        var guidExerciseId = Guid.Parse(exerciseId);

        var result =  await _simpleExerciseService.GetSimpleExercise(userId, guidExerciseId);

        return Ok(result);
    }
    
    [HttpGet(ApiRoutes.SimpleExercise.GetAll)]
    public async Task<IActionResult> GetSimpleExercises([FromRoute] int page, int size)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
            
        var userId = Guid.Parse(UtilsControllers.GetUserIdFromClaim(HttpContext));

        var result =  await _simpleExerciseService.GetSimpleExercises(userId, page, size);

        return Ok(result);
    }
    
    [HttpGet(ApiRoutes.SimpleExercise.GetAllForeign)]
    public async Task<IActionResult> GetForeignSimpleExercises([FromQuery] string userId, [FromRoute] int page, int size)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
            
        var jwtId = Guid.Parse(UtilsControllers.GetUserIdFromClaim(HttpContext));
        var id = Guid.Parse(userId);

        var result =  await _simpleExerciseService.GetForeignExercises(jwtId, id, page, size);

        return Ok(result);
    }

    [HttpDelete(ApiRoutes.SimpleExercise.Remove)]
    public async Task<IActionResult> RemoveSimpleExercise([FromQuery] string id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
            
        var userId = Guid.Parse(UtilsControllers.GetUserIdFromClaim(HttpContext));
        var exerciseId = Guid.Parse(id);

        await _simpleExerciseService.RemoveSimpleExercise(userId, exerciseId);

        return Ok();
    }
}