using FireConfig.Options;
using Microsoft.Extensions.Configuration;

namespace FireConfig;

public class MongoConfigurationSource : IConfigurationSource
{
    private readonly MongoConfigConnectionOptions _options;
    
    public MongoConfigurationSource(MongoConfigConnectionOptions options)
    {
        _options = options;
    }
    
    public IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        return new MongoConfigurationProvider(_options);
    }
}