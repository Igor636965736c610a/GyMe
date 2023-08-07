﻿using GymAppApi.Routes.v1;
using GymAppInfrastructure.Dtos.Exercise;
using GymAppInfrastructure.IServices;
using GymAppInfrastructure.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymAppApi.Controllers.v1;

[Authorize(Policy = "SSO")]
[Route("[controller]")]
public class ChartController : ControllerBase
{
    private readonly IChartService _chartService;
    public ChartController(IChartService chartService)
    {
        _chartService = chartService;
    }
    

    [HttpGet(ApiRoutes.Chart.GetById)]
    public async Task<IActionResult> GetChart([FromRoute]string exerciseId, [FromQuery]ChartOption option, [FromQuery]int period)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var id = Guid.Parse(exerciseId);

        var chart = await _chartService.Get(id, option, period);

        return Ok(chart);
    }
    
    [HttpGet(ApiRoutes.Chart.GetByType)]
    public async Task<IActionResult> GetChart([FromQuery]string userId, [FromQuery]ExercisesTypeDto exercisesType, [FromQuery]ChartOption option, [FromQuery]int period)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var owner = Guid.Parse(userId);

        var chart = await _chartService.Get(owner, exercisesType, option, period);

        return Ok(chart);
    }
    
    [HttpGet(ApiRoutes.Chart.GetAllByIds)]
    public async Task<IActionResult> GetChart([FromQuery] string userId, [FromQuery]IEnumerable<string> exercisesId, [FromQuery]ChartOption option, [FromQuery]int period)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var owner = Guid.Parse(userId);
        var ids = exercisesId.Select(x => Guid.Parse(x));

        var charts = await _chartService.Get(owner, ids, option, period);

        return Ok(charts);
    }
    
    [HttpGet(ApiRoutes.Chart.GetAllByTypes)]
    public async Task<IActionResult> GetChart([FromQuery]string userId, [FromQuery]IEnumerable<ExercisesTypeDto> exercisesType, [FromQuery]ChartOption option, [FromQuery]int period)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var owner = Guid.Parse(userId);

        var charts = await _chartService.Get(owner, exercisesType, option, period);

        return Ok(charts);
    }
}