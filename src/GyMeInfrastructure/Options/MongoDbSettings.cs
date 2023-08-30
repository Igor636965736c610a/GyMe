namespace GymAppInfrastructure.Options;

public class MongoDbSettings
{
    public string ConnectionURI { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
    public string ErrorsCollectionName { get; set; } = null!;
    public string PaymentMessagesCollectionName { get; set; } = null!;
}