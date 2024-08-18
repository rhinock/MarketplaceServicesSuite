using Carting.Core.CartAggregate;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace Carting.Infrastructure.Data;

public class AppDbContext
{
    private const string AdminDatabaseName = "admin";
    private const string MongoDbAuthMechanism = "SCRAM-SHA-1";
    private readonly MongoDbConfiguration _configuration;
    private readonly IMongoDatabase _database;

    public AppDbContext(IOptions<MongoDbConfiguration> options)
    {
        _configuration = options.Value;
        ArgumentNullException.ThrowIfNull(_configuration, nameof(_configuration));
        ArgumentException.ThrowIfNullOrWhiteSpace(_configuration.ConnectionString, nameof(_configuration.ConnectionString));
        ArgumentException.ThrowIfNullOrWhiteSpace(_configuration.Database, nameof(_configuration.Database));
        ArgumentException.ThrowIfNullOrWhiteSpace(_configuration.User, nameof(_configuration.User));
        ArgumentException.ThrowIfNullOrWhiteSpace(_configuration.Password, nameof(_configuration.Password));
        ArgumentException.ThrowIfNullOrWhiteSpace(_configuration.Host, nameof(_configuration.Host));

        var internalIdentity = new MongoInternalIdentity(AdminDatabaseName, _configuration.User);
        var passwordEvidence = new PasswordEvidence(_configuration.Password);
        var mongoCredential = new MongoCredential(MongoDbAuthMechanism, internalIdentity, passwordEvidence);
        
        var settings = new MongoClientSettings
        {
            Credential = mongoCredential,
            Server = new MongoServerAddress(_configuration.Host)
        };

        var client = new MongoClient(settings);
        _database = client.GetDatabase(_configuration.Database);

        if (!BsonClassMap.IsClassMapRegistered(typeof(Cart)))
        {
            BsonClassMap.RegisterClassMap<Cart>(cart =>
            {
                cart.AutoMap();

                cart
                    .MapIdProperty(x => x.Id)
                    .SetIdGenerator(StringObjectIdGenerator.Instance)
                    .SetSerializer(new StringSerializer(BsonType.ObjectId));
            });
        }
    }

    public IMongoCollection<Cart> Carts
    {
        get { return _database.GetCollection<Cart>("Carts"); }
    }
}