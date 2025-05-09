namespace Ambev.DeveloperEvaluation.ORM.MongoDB;

/// <summary>
/// MongoDB connection settings
/// </summary>
public class MongoDbSettings
{
    /// <summary>
    /// MongoDB connection string
    /// </summary>
    public string ConnectionString { get; set; }

    /// <summary>
    /// MongoDB database name
    /// </summary>
    public string DatabaseName { get; set; }
}