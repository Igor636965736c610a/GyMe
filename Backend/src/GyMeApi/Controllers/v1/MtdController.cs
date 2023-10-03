using GymAppApi.Controllers.HelperAttributes;
using GymAppInfrastructure.ApiResponses;
using GymAppInfrastructure.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymAppApi.Controllers.v1;

[SkipValidAccountCheck]
[Route("[controller]")]
public class MtdController : ControllerBase
{
    private readonly IJokeApiService _jokeApi;
    public MtdController(IJokeApiService jokeApi)
    {
        _jokeApi = jokeApi;
    }

    [AllowAnonymous]
    [HttpGet("get")]
    public async Task<IActionResult> GetJoke()
    {
        var joke = await _jokeApi.GetJoke("Dark");

        if (joke.Type == JokeType.single)
        {
            return Ok(joke.Joke);
        }

        return Ok(joke.Setup + "\n" + joke.Delivery);
    }
}