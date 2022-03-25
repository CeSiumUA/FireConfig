namespace FireConfig.Options;

public record MongoConfigConnectionOptions
{
    public const string MongoConfigConnectionOptionsName = nameof(MongoConfigConnectionOptions);
    
    public string ConnectionString { get; set; } = default!;
    
    public string DatabaseName { get; set; } = default!;

    public string CollectionName { get; set; } = default!;
    
    public ICollection<string>? Keys { get; set; } = new List<string>();
}