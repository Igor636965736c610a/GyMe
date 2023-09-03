﻿using GymAppInfrastructure.Models.InternalManagement;
using GymAppInfrastructure.Options;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace GymAppInfrastructure.Services.InternalManagement;

public class OpinionService
{
    private readonly IMongoCollection<Opinion> _opinionsCollection;

    public OpinionService(IOptionsMonitor<MongoDbSettings> mongoDbSettings)
    {
        MongoClient client = new MongoClient(mongoDbSettings.CurrentValue.ConnectionURI);
        IMongoDatabase database = client.GetDatabase(mongoDbSettings.CurrentValue.DatabaseName);
        _opinionsCollection = database.GetCollection<Opinion>(mongoDbSettings.CurrentValue.OpinionsCollectionName);
    }
    
    public async Task Add(Opinion opinion)
    {
        await _opinionsCollection.InsertOneAsync(opinion);
    }
}