namespace Carting.Infrastructure.Data;

public class MongoDbConfiguration
{
    public string ConnectionString { get; set; } = null!;
    public string Database { get; set; } = null!;
    public string User { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Host {get;set;} = null!;
}