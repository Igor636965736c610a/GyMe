using GyMeCore.IRepo;
using GyMeCore.Models.Entities;
using GyMeInfrastructure.Models.ReactionsAndComments.BodyRequest;
using GyMeInfrastructure.Models.ReactionsAndComments;
using GyMeInfrastructure.Options;
using Microsoft.EntityFrameworkCore;

namespace GyMeInfrastructure.Repo;

internal class SimpleExerciseRepo : ISimpleExerciseRepo
{
    private readonly GyMePostgresContext _gyMePostgresContext;
    public SimpleExerciseRepo(GyMePostgresContext gyMePostgresContext)
    {
        _gyMePostgresContext = gyMePostgresContext;
    }
    
    public async Task<SimpleExercise?> Get(Guid id)
        => await _gyMePostgresContext.SimpleExercises
            .Include(x => x.Series)
            .Include(x => x.Reactions
                .OrderBy(z => z.ReactionType == ReactionType.Image.ToStringFast())
                .ThenBy(z => z.TimeStamp)
                .Take(3))
            .ThenInclude(x => x.User)
            .FirstOrDefaultAsync(x => x.Id == id);

    public async Task<List<SimpleExercise>> GetAll(Guid exerciseId, int page, int size)
        => await _gyMePostgresContext.SimpleExercises.Where(x => x.ExerciseId == exerciseId)
            .OrderBy(x => x.TimeStamp)
            .Include(x => x.Series)
            .Include(x => x.Reactions
                .OrderBy(z => z.ReactionType == ReactionType.Image.ToStringFast())
                .ThenBy(z => z.TimeStamp)
                .Take(3))
            .ThenInclude(x => x.User)
            .Skip(page*size)
            .Take(size)
            .ToListAsync();

    public async Task Create(SimpleExercise exercise)
    {
        await _gyMePostgresContext.SimpleExercises.AddAsync(exercise);
        await _gyMePostgresContext.SaveChangesAsync();
    }

    public async Task Update(SimpleExercise exercise)
    {
        _gyMePostgresContext.SimpleExercises.Update(exercise);
        await _gyMePostgresContext.SaveChangesAsync();
    }

    public async Task Remove(SimpleExercise exercise)
    {
        _gyMePostgresContext.SimpleExercises.Remove(exercise);
        await _gyMePostgresContext.SaveChangesAsync();
    }
}