using GymAppInfrastructure.Options;
using GymAppInfrastructure.Models.InternalManagement;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace GymAppInfrastructure.Services.InternalManagement;

public class ErrorService
{
    private readonly IMongoCollection<Error> _errorsCollection;
    
    public ErrorService(IOptions<MongoDbErrors> mongoDbSettings) {
        MongoClient client = new MongoClient(mongoDbSettings.Value.ConnectionURI);
        IMongoDatabase database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);
        _errorsCollection = database.GetCollection<Error>(mongoDbSettings.Value.CollectionName);
    }

    public async Task Add(Error error)
    {
        await _errorsCollection.InsertOneAsync(error);
    }
}