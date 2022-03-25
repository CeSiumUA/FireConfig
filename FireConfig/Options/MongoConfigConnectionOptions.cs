namespace FireConfig.Options;

public record MongoConfigConnectionOptions(
    string ConnectionString,
    string DatabaseName,
    string CollectionName,
    ICollection<string>? Keys = null
);