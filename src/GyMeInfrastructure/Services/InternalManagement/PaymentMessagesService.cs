using GymAppInfrastructure.Models.InternalManagement;
using GymAppInfrastructure.Options;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace GymAppInfrastructure.Services.InternalManagement;

public class PaymentMessagesService
{
    private readonly IMongoCollection<PaymentMessage> _paymentMessagesCollection;

    public PaymentMessagesService(IOptions<MongoDbSettings> mongoDbSettings)
    {
        MongoClient client = new MongoClient(mongoDbSettings.Value.ConnectionURI);
        IMongoDatabase database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);
        _paymentMessagesCollection = database.GetCollection<PaymentMessage>(mongoDbSettings.Value.PaymentMessagesCollectionName);
    }
    
    public async Task Add(PaymentMessage paymentMessage)
    {
        await _paymentMessagesCollection.InsertOneAsync(paymentMessage);
    }
}