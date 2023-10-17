using GyMeApplication.IServices;
using GyMeWebApi.Routes.v1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GyMeWebApi.Controllers.v1;

[Authorize(Policy = "AppSys")]
[Route("[controller]")]
public class MainPageController : ControllerBase
{
    private readonly IMainPageService _mainPageService;

    public MainPageController(IMainPageService mainPageService)
    {
        _mainPageService = mainPageService;
    }
    
    [HttpGet(ApiRoutes.MainPage.GetNewSimpleExerciseElements)]
    public async Task<IActionResult> GetNewSimpleExerciseElements([FromQuery]int page, [FromQuery]int size)
    {
        if (!ModelState.IsValid)
            return BadRequest("Invalid data");

        var elements = await _mainPageService.GetNewSimpleExercisesForMainPage(page, size);

        return Ok(elements);
    }
    
    [HttpGet(ApiRoutes.MainPage.GetPastSimpleExerciseElements)]
    public async Task<IActionResult> GetPastSimpleExerciseElements([FromQuery]int page, [FromQuery]int size)
    {
        if (!ModelState.IsValid)
            return BadRequest("Invalid data");

        var elements = await _mainPageService.GetPastSimpleExercisesForMainPage(page, size);

        return Ok(elements);
    }
}