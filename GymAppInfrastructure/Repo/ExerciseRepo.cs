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
    
    public async Task<Exercise?> Get(User user, ExercisesType exercisesType)
        => await _gymAppContext.Exercises.FirstOrDefaultAsync(x => x.User == user && x.Shown && x.ExercisesType == exercisesType);

    public async Task<IEnumerable<Exercise>> Get(User user)
        => await Task.FromResult(_gymAppContext.Exercises.Where(x => x.User == user && x.Shown).OrderBy(x => x.Position));

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

    public async Task<bool> Hide(Exercise exercise)
    {
        exercise.Shown = false;
        return await Update(exercise);
    }
}