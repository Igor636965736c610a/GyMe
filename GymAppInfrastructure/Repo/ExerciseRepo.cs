using GymAppCore.IRepo;
using GymAppCore.Models.Entities;
using GymAppInfrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace GymAppInfrastructure.Repo;

public class ExerciseRepo : IExerciseRepo
{
    private readonly GymAppContext _gymAppContext;
    public ExerciseRepo(GymAppContext gymAppContext)
    {
        _gymAppContext = gymAppContext;
    }
    
    public async Task<Exercise?> Get(Guid exerciseId)
        => await _gymAppContext.Exercises.FirstOrDefaultAsync(x => x.Id == exerciseId);

    public async Task<Exercise?> Get(Guid userId, ExercisesType exercisesType)
        => await _gymAppContext.Exercises.FirstOrDefaultAsync(x =>
            x.UserId == userId && x.ExercisesType == exercisesType);

    public async Task<List<Exercise>> GetAll(Guid userId, int page, int size)
        => await _gymAppContext.Exercises.Where(x => x.UserId == userId).OrderBy(x => x.Position)
            .Skip(page * size)
            .Take(size).ToListAsync();

    public async Task<List<Exercise>> GetAll(Guid userId)
        => await _gymAppContext.Exercises.Where(x => x.UserId == userId).OrderBy(x => x.Position).ToListAsync();
    
    public async Task<Dictionary<Guid, SimpleExercise?>> GetMaxRep(IEnumerable<Guid> exercisesId)
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
    public async Task<SimpleExercise?> GetMaxRep(Guid exerciseId)
        => await _gymAppContext.Series
            .Where(s => s.SimpleExercise.ExerciseId == exerciseId)
            .OrderByDescending(s => s.Weight)
            .ThenByDescending(s => s.NumberOfRepetitions)
            .Select(s => s.SimpleExercise)
            .FirstOrDefaultAsync();
    
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
    
    public async Task<bool> Create(Exercise exercise)
    {
        await _gymAppContext.Exercises.AddAsync(exercise);
        return await UtilsRepo.Save(_gymAppContext);
    }

    public async Task<bool> Update(Exercise exercise)
    {
        await Task.FromResult(_gymAppContext.Update(exercise));
        return await UtilsRepo.Save(_gymAppContext);
    }

    public async Task<bool> Update(List<Exercise> exercises)
    {
        _gymAppContext.UpdateRange(exercises);
        return await UtilsRepo.Save(_gymAppContext);
    }

    public async Task<bool> Remove(Exercise exercise)
    {
        _gymAppContext.Remove(exercise);
        return await UtilsRepo.Save(_gymAppContext);
    }
}