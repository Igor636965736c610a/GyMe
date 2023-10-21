using GyMeApplication.Models.Exercise;
using GyMeApplication.IServices;
using GyMeWebApi.Routes.v1;
using Microsoft.AspNetCore.Authorization;
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
        
        var location = Url.Action("GetExercise",new { id = exerciseId.ToString() })!;

        return Created(location, exerciseId.ToString());
    }

    [HttpGet(ApiRoutes.Exercise.Get)]
    public async Task<IActionResult> GetExercise([FromRoute]Guid id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var exercise = await _exerciseService.Get(id);
        
        return Ok(exercise);
    }

    [HttpGet(ApiRoutes.Exercise.GetAll)]
    public async Task<IActionResult> GetAll([FromQuery]Guid userId, [FromQuery]int page, [FromQuery]int size)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var exercises = await _exerciseService.Get(userId, page, size);

        return Ok(exercises);
    }

    [HttpPut(ApiRoutes.Exercise.Update)]
    public async Task<IActionResult> Update([FromBody]PutExerciseDto putExerciseDto, [FromRoute]Guid id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var exercise = await _exerciseService.Update(id, putExerciseDto);
        
        return Ok(exercise);
    }

    [HttpDelete(ApiRoutes.Exercise.Remove)]
    public async Task<IActionResult> Remove([FromRoute]Guid id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        await _exerciseService.Remove(id);
        
        return Ok();
    }
}