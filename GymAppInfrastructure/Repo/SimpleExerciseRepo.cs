﻿using GymAppCore.IRepo;
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

    public async Task<List<SimpleExercise>> GetAll(Guid userId)
        => await _gymAppContext.SimpleExercises.Where(x => x.UserId == userId).Include(x => x.Series).ToListAsync();

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
}