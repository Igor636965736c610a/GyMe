using GyMeApplication.IServices;
using GyMeApplication.Models.Exercise;
using GyMeApplication.Options;
using GyMeWebApi.Routes.v1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GyMeWebApi.Controllers.v1;

[Authorize(Policy = "AppSys")]
[Route("[controller]")]
public class ChartGeneratorController : ControllerBase
{
    private readonly IChartGeneratorService _chartGeneratorService;

    public ChartGeneratorController(IChartGeneratorService chartGeneratorService)
    {
        _chartGeneratorService = chartGeneratorService;
    }

    [HttpGet(ApiRoutes.Chart.GetById)]
    public async Task<IActionResult> GetGeneratedChart([FromQuery]string exerciseId, [FromQuery]ChartOption chartOption, [FromQuery]int period)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var chart = await _chartGeneratorService.Get(Guid.Parse(exerciseId), chartOption, period);

        return Ok(chart);
    }
    
    [HttpGet(ApiRoutes.Chart.GetByType)]
    public async Task<IActionResult> GetGeneratedChart([FromQuery]string userId, [FromQuery]ExercisesTypeDto exercisesTypeDto, [FromQuery]ChartOption chartOption, [FromQuery]int period)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var chart = await _chartGeneratorService.Get(Guid.Parse(userId), exercisesTypeDto, chartOption, period);

        return Ok(chart);
    }
    
    [HttpGet(ApiRoutes.Chart.GetAllByIds)]
    public async Task<IActionResult> GetGeneratedCharts([FromQuery]string userId, [FromQuery]IEnumerable<string> exercisesIds, [FromQuery]ChartOption chartOption, [FromQuery]int period)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var charts = await _chartGeneratorService.Get(Guid.Parse(userId), exercisesIds.Select(Guid.Parse), chartOption, period);

        return Ok(charts);
    }
    
    [HttpGet(ApiRoutes.Chart.GetAllByTypes)]
    public async Task<IActionResult> GetGeneratedCharts([FromQuery]string userId, [FromQuery]IEnumerable<ExercisesTypeDto> exercisesTypeDto, [FromQuery]ChartOption chartOption, [FromQuery]int period)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var charts = await _chartGeneratorService.Get(Guid.Parse(userId), exercisesTypeDto, chartOption, period);

        return Ok(charts);
    }
}