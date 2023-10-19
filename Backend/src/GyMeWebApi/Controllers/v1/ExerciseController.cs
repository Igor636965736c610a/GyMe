using GyMeCore.Models.Entities;
using GyMeApplication.Models.Exercise;
using GyMeApplication.IServices;
using GyMeWebApi.Routes.v1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace GyMeWebApi.Controllers.v1;

[Authorize(Policy = "AppSys")]
[Route("[controller]")]
public class ExerciseController : ControllerBase
{
    private readonly IExerciseService _exerciseService;
    public ExerciseController(IExerciseService exerciseService)
    {
        _exerciseService = exerciseService;
    }

    [HttpPost(ApiRoutes.Exercise.Create)]
    public async Task<IActionResult> CreateExercise([FromBody]PostExerciseDto postExerciseDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var exerciseId = await _exerciseService.Create(postExerciseDto);

        return Ok(exerciseId.ToString());   
    }

    [HttpGet(ApiRoutes.Exercise.Get)]
    public async Task<IActionResult> Get([FromRoute] string id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var exerciseId = Guid.Parse(id);
        
        var exercise = await _exerciseService.Get(exerciseId);
        
        return Ok(exercise);
    }

    [HttpGet(ApiRoutes.Exercise.GetAll)]
    public async Task<IActionResult> GetAll([FromQuery] string userId,[FromQuery] int page,[FromQuery] int size)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var id =  Guid.Parse(userId);

        var exercises = await _exerciseService.Get(id, page, size);

        return Ok(exercises);
    }

    [HttpPut(ApiRoutes.Exercise.Update)]
    public async Task<IActionResult> Update([FromBody] PutExerciseDto putExerciseDto, [FromRoute] string id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var exerciseId = Guid.Parse(id);
        
        await _exerciseService.Update(exerciseId, putExerciseDto);
        
        return Ok();
    }

    [HttpDelete(ApiRoutes.Exercise.Remove)]
    public async Task<IActionResult> Remove([FromRoute] string id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var exerciseId = Guid.Parse(id);
        
        await _exerciseService.Remove(exerciseId);
        
        return Ok();
    }
}