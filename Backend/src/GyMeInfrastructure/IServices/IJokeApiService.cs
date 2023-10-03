using GyMeInfrastructure.ApiResponses;
using Refit;

namespace GyMeInfrastructure.IServices;

public interface IJokeApiService
{
    [Get("/joke/{category}")]
    Task<JokeResponse> GetJoke(string category);
}