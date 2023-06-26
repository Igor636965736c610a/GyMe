using GymAppApi.Routes.v1;
using GymAppInfrastructure.IServices;
using GymAppInfrastructure.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymAppApi.Controllers.v1;

[Authorize]
[Route("[controller]")]
public class ChartController : ControllerBase
{
    private readonly IChartService _chartService;
    public ChartController(IChartService chartService)
    {
        _chartService = chartService;
    }
    

    [HttpGet(ApiRoutes.Chart.Get)]
    public async Task<IActionResult> GetChart([FromRoute]string exerciseId, [FromQuery]ChartOption option, [FromQuery]int period)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var jwtId = Guid.Parse(UtilsControllers.GetUserIdFromClaim(HttpContext));
        var id = Guid.Parse(exerciseId);

        var chart = await _chartService.Get(jwtId, id, option, period);

        return Ok(chart);
    }
    
    [HttpGet(ApiRoutes.Chart.GetAll)]
    public async Task<IActionResult> GetChart([FromQuery] string userId, [FromQuery]IEnumerable<string> exercisesId, [FromQuery]ChartOption option, [FromQuery]int period)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var jwtId = Guid.Parse(UtilsControllers.GetUserIdFromClaim(HttpContext));
        var owner = Guid.Parse(userId);
        var ids = exercisesId.Select(x => Guid.Parse(x));

        var charts = await _chartService.Get(jwtId, owner, ids, option, period);

        return Ok(charts);
    }
}