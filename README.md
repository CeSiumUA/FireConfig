[![.NET](https://github.com/CeSiumUA/FireConfig/actions/workflows/dotnet-build.yaml/badge.svg)](https://github.com/CeSiumUA/FireConfig/actions/workflows/dotnet-build.yaml)

# FireConfig

This project aims to extend basic configuration methods for ASP.NET Core
Currently, only MongoDb is added as configuratin provider
As for now, I consider to be added: Telegram (via BOT)

## Installation

To install via dotnet CLI:

```PowerShell
dotnet add package FireConfig
```

Via Package Manager:

```PowerShell
Install-Package FireConfig
```

[NuGet](https://www.nuget.org/packages/FireConfig/)

## Usage

In MongoDb, you need to structure records like this:

```JSON
{
    "_id": "..."
    "ProjectCommon": {
        "ConnectionStrings": {
            "MongoDb": "<Your connection Id string>",
            "NpgSql": "<Your connection Id string>"
        },
        "TokenOptions": {
            "SigningKey": "<Your very very secret key>"
        },
        "SomeTestOptions": [
            {
                "Test1": 1
            }, {
                "Test2": 2
            }, "Test3"]
    }
}
```

ProjectCommon is a "Key" here, you could specify your keys in options (more information below)
All the keys are trimmed after readingm, they are used to limit records which your app will read from DB
Something similar you could see while using Azure AppConfiguration

```C#
// initialize options
var mongoConfigOptions = configuration
                .GetSection(MongoConfigConnectionOptions.MongoConfigConnectionOptionsName)
                .Get<MongoConfigConnectionOptions>();

// add new configuration to ConfigureHostBuilder
builder.ConfigureAppConfiguration(configBuilder =>
            {
                configBuilder.AddMongoDbConfiguration(mongoConfigOptions);
            });
```

Typical options loks like:

```JSON
"MongoConfigConnectionOptions": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "configdb",
    "CollectionName": "configuration",
    "Keys": [
      "ProjectCommon"
    ]
  }
```