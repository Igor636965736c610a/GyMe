using GyMeApplication.Models.SimpleExercise;
using GyMeApplication.IServices;
using GyMeWebApi.Routes.v1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GyMeWebApi.Controllers.v1;

[Authorize(Policy = "AppSys")]
[Route("[controller]")]
public class SimpleExerciseController : ControllerBase
{
    private readonly ISimpleExerciseService _simpleExerciseService;
    public SimpleExerciseController(ISimpleExerciseService simpleExerciseService)
    {
        _simpleExerciseService = simpleExerciseService;
    }

    [HttpPost(ApiRoutes.SimpleExercise.Create)]
    public async Task<IActionResult> CreateSimpleExercise([FromBody]PostSimpleExerciseDto postSimpleExerciseDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var simpleExerciseId = await _simpleExerciseService.Create(postSimpleExerciseDto);
        
        var location = Url.Action("GetSimpleExercise",new { id = simpleExerciseId })!;

        return Created(location, simpleExerciseId.ToString());
    }

    [HttpPut(ApiRoutes.SimpleExercise.Update)]
    public async Task<IActionResult> UpdateSimpleExercise([FromRoute]Guid id, [FromBody]PutSimpleExerciseDto putSimpleExerciseDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var simpleExercise = await _simpleExerciseService.Update(id, putSimpleExerciseDto);

        return Ok(simpleExercise);
    }

    [HttpGet(ApiRoutes.SimpleExercise.Get)]
    public async Task<IActionResult> GetSimpleExercise([FromRoute]Guid id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result =  await _simpleExerciseService.Get(id);

        return Ok(result);
    }
    
    [HttpGet(ApiRoutes.SimpleExercise.GetAll)]
    public async Task<IActionResult> GetSimpleExercises([FromQuery]Guid exerciseId, [FromQuery]int page, [FromQuery]int size)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result =  await _simpleExerciseService.Get(exerciseId, page, size);

        return Ok(result);
    }

    [HttpDelete(ApiRoutes.SimpleExercise.Remove)]
    public async Task<IActionResult> RemoveSimpleExercise([FromRoute]Guid id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _simpleExerciseService.Remove(id);

        return Ok();
    }
}