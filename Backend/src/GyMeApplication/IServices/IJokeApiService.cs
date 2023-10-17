using GyMeApplication.ApiResponses;
using Refit;

namespace GyMeApplication.IServices;

public interface IJokeApiService
{
    [Get("/joke/{category}")]
    Task<JokeResponse> GetJoke(string category);
}