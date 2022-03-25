using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using FireConfig.Options;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FireConfig;

public class MongoConfigurationProvider : ConfigurationProvider
{
    private readonly MongoConfigConnectionOptions _options;
    public MongoConfigurationProvider(MongoConfigConnectionOptions options)
    {
        this._options = options;
    }
    public override void Load()
    {
        var client = new MongoClient(_options.ConnectionString);

        var dataBase = client.GetDatabase(_options.DatabaseName);

        var collection = dataBase.GetCollection<BsonDocument>(_options.CollectionName);

        var filterDefinitions = _options.Keys?.Select(x => Builders<BsonDocument>.Filter.Exists(x)).ToArray() ?? Array.Empty<FilterDefinition<BsonDocument>>();

        var keysCollection = collection.Find(Builders<BsonDocument>.Filter.Or(filterDefinitions)).ToList();
        
        ParseBsonDocuments(keysCollection);
        
        base.Load();
    }

    private void ParseBsonDocuments(IEnumerable<BsonDocument> documents)
    {
        foreach (var doc in documents)
        {
            var json = doc.ToJson();
            if (!string.IsNullOrEmpty(json))
            {
                var jNode = JsonNode.Parse(json);
                if (jNode != null)
                {
                    ParseObject(jNode);
                }
            }
        }
    }
    
    void ParseObject(JsonNode jNode)
    {
        if (jNode is JsonObject jObject)
        {
            foreach (var jObjectNode in jObject)
            {
                var node = jObjectNode.Value;
                if (node != null)
                {
                    ParseObject(node);   
                }
            }
        }
        else if (jNode is JsonArray jArray)
        {
            foreach (var element in jArray)
            {
                if (element != null)
                {
                    ParseObject(element);
                }
            }
        }
        else
        {
            var regex = new Regex(@"\[\d+\]");
            var path = jNode.GetPath().Replace("$.", string.Empty).Replace(".", ":");
            var matches = regex.Matches(path);
            for (int x = 0; x < matches.Count; x++)
            {
                var match = matches[x];
                var digits = match.Value.Replace("[", string.Empty).Replace("]", string.Empty);
                path = path.Replace(match.Value, $":{digits}");
            }
            var value = jNode.AsValue().ToString();
            Data.Add(path, value);
        }
    }
}