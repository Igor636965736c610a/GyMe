using GymAppInfrastructure.ApiResponses;
using Refit;

namespace GymAppInfrastructure.IServices;

public interface IJokeApiService
{
    [Get("/joke/{category}")]
    Task<JokeResponse> GetJoke(string category);
}