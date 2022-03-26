using FireConfig.Options;
using Microsoft.Extensions.Configuration;

namespace FireConfig.MongoDb;

public static class MongoDbConfigurationExtensions
{
    public static IConfigurationBuilder AddMongoDbConfiguration(this IConfigurationBuilder builder, MongoConfigConnectionOptions options)
    {
        return builder.Add(new MongoConfigurationSource(options));
    }
}