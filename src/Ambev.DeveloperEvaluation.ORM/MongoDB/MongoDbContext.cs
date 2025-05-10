using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Ambev.DeveloperEvaluation.ORM.MongoDB;

/// <summary>
/// MongoDB connection context
/// </summary>
public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    /// <summary>
    /// Initializes a new instance of the MongoDB context
    /// </summary>
    /// <param name="settings">MongoDB connection settings</param>
    public MongoDbContext(IOptions<MongoDbSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        _database = client.GetDatabase(settings.Value.DatabaseName);
    }

    /// <summary>
    /// Customers collection in MongoDB
    /// </summary>
    public IMongoCollection<Customer> Customers => _database.GetCollection<Customer>("Customers");
}
