using GymAppInfrastructure.Models.InternalManagement;
using GymAppInfrastructure.Options;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace GymAppInfrastructure.Services.InternalManagement;

public class PaymentMessagesService
{
    private readonly IMongoCollection<PaymentMessage> _paymentMessagesCollection;

    public PaymentMessagesService(IOptionsMonitor<MongoDbSettings> mongoDbSettings)
    {
        MongoClient client = new MongoClient(mongoDbSettings.CurrentValue.ConnectionURI);
        IMongoDatabase database = client.GetDatabase(mongoDbSettings.CurrentValue.DatabaseName);
        _paymentMessagesCollection = database.GetCollection<PaymentMessage>(mongoDbSettings.CurrentValue.PaymentMessagesCollectionName);
    }
    
    public async Task Add(PaymentMessage paymentMessage)
    {
        await _paymentMessagesCollection.InsertOneAsync(paymentMessage);
    }
}