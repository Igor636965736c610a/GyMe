using GymAppApi.Routes.v1;
using GymAppInfrastructure.Models.SimpleExercise;
using GymAppInfrastructure.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymAppApi.Controllers.v1;

[Authorize(Policy = "SSO")]
[Route("[controller]")]
public class SimpleExerciseController : ControllerBase
{
    private readonly ISimpleExerciseService _simpleExerciseService;
    public SimpleExerciseController(ISimpleExerciseService simpleExerciseService)
    {
        _simpleExerciseService = simpleExerciseService;
    }

    [HttpPost(ApiRoutes.SimpleExercise.Create)]
    public async Task<IActionResult> CreateSimpleExercise([FromBody] PostSimpleExerciseDto postSimpleExerciseDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _simpleExerciseService.Create(postSimpleExerciseDto);

        return Ok();
    }

    [HttpPut(ApiRoutes.SimpleExercise.Update)]
    public async Task<IActionResult> UpdateSimpleExercise([FromRoute] string id, [FromBody] PutSimpleExerciseDto putSimpleExerciseDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
            
        var exerciseId = Guid.Parse(id);

        await _simpleExerciseService.Update(exerciseId, putSimpleExerciseDto);
        
        return Ok();
    }

    [HttpGet(ApiRoutes.SimpleExercise.Get)]
    public async Task<IActionResult> GetSimpleExercise([FromRoute] string id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
            
        var guidExerciseId = Guid.Parse(id);

        var result =  await _simpleExerciseService.Get(guidExerciseId);

        return Ok(result);
    }
    
    [HttpGet(ApiRoutes.SimpleExercise.GetAll)]
    public async Task<IActionResult> GetForeignSimpleExercises([FromQuery] string exerciseId, [FromQuery] string userId,[FromQuery] int page,[FromQuery] int size)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
            
        var id = Guid.Parse(userId);
        var parseExerciseId = Guid.Parse(exerciseId);

        var result =  await _simpleExerciseService.Get(id, parseExerciseId, page, size);

        return Ok(result);
    }

    [HttpDelete(ApiRoutes.SimpleExercise.Remove)]
    public async Task<IActionResult> RemoveSimpleExercise([FromRoute] string id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
            
        var exerciseId = Guid.Parse(id);

        await _simpleExerciseService.Remove(exerciseId);

        return Ok();
    }
}