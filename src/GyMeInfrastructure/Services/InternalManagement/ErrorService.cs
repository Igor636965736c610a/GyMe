using GymAppInfrastructure.Options;
using GymAppInfrastructure.Models.InternalManagement;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace GymAppInfrastructure.Services.InternalManagement;

public class ErrorService
{
    private readonly IMongoCollection<Error> _errorsCollection;
    
    public ErrorService(IOptionsMonitor<MongoDbSettings> mongoDbSettings) {
        MongoClient client = new MongoClient(mongoDbSettings.CurrentValue.ConnectionURI);
        IMongoDatabase database = client.GetDatabase(mongoDbSettings.CurrentValue.DatabaseName);
        _errorsCollection = database.GetCollection<Error>(mongoDbSettings.CurrentValue.ErrorsCollectionName);
    }

    public async Task Add(Error error)
    {
        await _errorsCollection.InsertOneAsync(error);
    }
}