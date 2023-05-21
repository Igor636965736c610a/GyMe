using GymAppCore.IRepo;
using GymAppCore.Models.Entities;
using GymAppInfrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace GymAppInfrastructure.Repo;

public class SimpleExerciseRepo : ISimpleExerciseRepo
{
    private readonly GymAppContext _gymAppContext;
    public SimpleExerciseRepo(GymAppContext gymAppContext)
    {
        _gymAppContext = gymAppContext;
    }
    
    public async Task<SimpleExercise?> Get(Guid id)
        => await _gymAppContext.SimpleExercises.Include(x => x.Series).FirstOrDefaultAsync(x => x.Id == id);

    public async Task<List<SimpleExercise>> GetAll(Guid userId, Guid exerciseId, int page, int size)
        => await _gymAppContext.SimpleExercises.Where(x => x.UserId == userId && x.ExerciseId == exerciseId)
            .OrderBy(x => x.Date)
            .Include(x => x.Series)
            .Skip(page*size)
            .Take(size)
            .ToListAsync();

    public async Task<List<SimpleExercise>> GetAll(Guid userId, Guid exerciseId)
        => await _gymAppContext.SimpleExercises.Where(x => x.UserId == userId && x.ExerciseId == exerciseId).Include(x => x.Series).ToListAsync();

    public async Task<Dictionary<Guid, SimpleExercise?>> GetMaxRepForExercises(IEnumerable<Guid> exercisesId)
        => await _gymAppContext.Series
            .Where(x => exercisesId.Contains(x.SimpleExercise.ExerciseId))
            .OrderByDescending(s => s.Weight)
            .ThenByDescending(s => s.NumberOfRepetitions)
            .GroupBy(x => x.SimpleExercise.ExerciseId)
            .Select(x => new
            {
                SimpleExercise = x.Select(y => y.SimpleExercise).FirstOrDefault(),
                exerciseId = x.Key
            })
            .ToDictionaryAsync(x => x.exerciseId, x => x.SimpleExercise);
    public async Task<SimpleExercise?> GetMaxRepForExercise(Guid exerciseId)
        => await _gymAppContext.Series
             .Where(s => s.SimpleExercise.ExerciseId == exerciseId)
             .OrderByDescending(s => s.Weight)
             .ThenByDescending(s => s.NumberOfRepetitions)
             .Select(s => s.SimpleExercise)
             .FirstOrDefaultAsync();

    //public async Task<Dictionary<Guid, SimpleExercise?>> GetMaxRepForExercises(IEnumerable<Guid> simpleExercisesId)

    public async Task<Dictionary<Exercise, int>> GetScoresss(IEnumerable<Guid> exercisesId, int size,
        Func<IEnumerable<Series>, int> calculate)
        => await _gymAppContext.Series
            .Where(x => exercisesId.Contains(x.SimpleExercise.ExerciseId))
            .GroupBy(x => x.SimpleExercise.Exercise)
            .Select(x => new
            {
                Score = x.GroupBy(y => y.SimpleExercise).OrderBy(z => z.Key.Date).Take(size).Sum(t => calculate(t)),
                Exercise = x.Key
            }).ToDictionaryAsync(x => x.Exercise, x => x.Score);
            
    
    public async Task<Dictionary<Exercise, int>> GetScoress(IEnumerable<Guid> exercisesId, int size,
        Func<IEnumerable<Series>, int> calculate)
        => await _gymAppContext.SimpleExercises
            .Where(x => exercisesId.Contains(x.ExerciseId))
            .Include(x => x.Exercise)
            .GroupBy(x => x.Exercise)
            .ToDictionaryAsync(
                g => g.Key,
                g => g
                    .OrderBy(x => x.Date)
                    .Take(size)
                    .Sum(x => calculate(x.Series))
                );


    public async Task<Dictionary<Exercise, int>> GetScore(Guid exerciseId, int size,
        Func<IEnumerable<Series>, int> calculate)
        => await _gymAppContext.Series
            .Where(x => x.SimpleExercise.ExerciseId == exerciseId)
            .OrderBy(x => x.SimpleExercise.Date)
            .Take(size)
            .GroupBy(x => x.SimpleExercise.Exercise)
            .ToDictionaryAsync(x => x.Key, x => calculate(x));
            

    public async Task<bool> Create(SimpleExercise exercise)
    {
        await _gymAppContext.SimpleExercises.AddAsync(exercise);
        return await UtilsRepo.Save(_gymAppContext);
    }

    public async Task<bool> Update(SimpleExercise exercise)
    {
        _gymAppContext.SimpleExercises.Update(exercise);
        return await UtilsRepo.Save(_gymAppContext);
    }

    public async Task<bool> Remove(SimpleExercise exercise)
    {
        _gymAppContext.SimpleExercises.Remove(exercise);
        return await UtilsRepo.Save(_gymAppContext);
    }

    public static int CalculateScore(int weight, int reps)
        => (int)Math.Round(weight / (1.0278 - 0.0278 * reps), 2, MidpointRounding.AwayFromZero);
}