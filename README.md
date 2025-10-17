# Utils.DotNetCore.IServiceCollection - Get Config Value extension

A small utility library that extends [`Microsoft.Extensions.Configuration`](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration.iconfiguration) with strongly-typed helpers for retrieving configuration values in a safe and expressive way.

## Features

- **Strongly-typed configuration access** – Retrieve values directly as `int`, `bool`, `string`, custom POCOs, etc.
- **Nested key lookup** – Pass multiple keys (`params string[]`) to walk through hierarchical configuration sections.
- **Fail-fast or fallback options**:
  - `GetConfigurationValue<T>` – throws if the key does not exist or cannot be bound.
  - `GetConfigurationValueOrDefault<T>` – returns a default value if the key is missing or null.

## Installation

Add a reference to your project:

```bash
dotnet add package Utils.DotNetCore.IServiceCollection
```

## Config file example
```json
{
  "ConnectionStrings": {
    "Default": "Server=myserver;Database=mydb;User Id=sa;Password=secret;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning"
    }
  },
  "FeatureFlags": {
    "EnableNewUI": true
  }
}
```

## Basic usage
```csharp
namespace Sample.Use.Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddJsonFile("config.json", optional: false, reloadOnChange: true);

            IConfiguration configuration = builder.Configuration;

            var connectionString = configuration.GetConfigurationValue<string>("ConnectionStrings", "Default");
            System.Console.WriteLine(string.Concat("ConnectionStrings", ":", "Default", "=", connectionString));

            var enableNewUI = configuration.GetConfigurationValue<bool>("FeatureFlags", "EnableNewUI");
            System.Console.WriteLine(string.Concat("FeatureFlags", ":", "EnableNewUI", "=", enableNewUI));

            var maxRetries = configuration.GetConfigurationValueOrDefault(3, "AppSettings", "MaxRetries");
            System.Console.WriteLine(string.Concat("AppSettings", ":", "MaxRetries", "=", maxRetries));

            var loggingOptions = configuration.GetConfigurationValue<LoggingOptions>("Logging");
            System.Console.WriteLine(string.Concat("Logging", "=", loggingOptions.LogLevel["Default"]));

            System.Console.ReadLine();
        }
    }

    internal class LoggingOptions
    {
        public Dictionary<string, string> LogLevel { get; set; } = new();
    }
}
```

## Nested section access
```json
{
  "AppSettings": {
    "Logging": {
      "Enabled": true,
      "Level": "Debug"
    }
  }
}
```

```csharp
bool loggingEnabled = config.GetConfigurationValue<bool>("AppSettings", "Logging", "Enabled");
string logLevel = config.GetConfigurationValue<string>("AppSettings", "Logging", "Level");
```

## Using defaults for missed keys
```csharp
int timeout = config.GetConfigurationValueOrDefault(30, "AppSettings", "Timeout");
```

## Binding to complex types
```csharp
public class LoggingOptions
{
    public bool Enabled { get; set; }
    public string Level { get; set; }
}

LoggingOptions options = config.GetConfigurationValue<LoggingOptions>("AppSettings", "Logging");
```

## Comparison
| Feature                           | `IConfiguration.GetValue<T>` (built-in) | `GetConfigurationValue<T>` (this library) | `GetConfigurationValueOrDefault<T>` (this library) |
| --------------------------------- | --------------------------------------- | ----------------------------------------- | -------------------------------------------------- |
| Supports nested hierarchical keys | ❌ Only colon-delimited string keys      | ✅ Accepts multiple keys via params array  | ✅ Accepts multiple keys via params array           |
| Throws if key does not exist      | ❌ Returns default(T) silently           | ✅ Throws `KeyNotFoundException`           | ❌ Returns provided default value                   |
| Throws if binding fails           | ❌ Returns default(T) silently           | ✅ Throws `InvalidOperationException`      | ❌ Returns provided default value                   |
| Provides safe defaults            | ✅ Requires specifying default(T) inline | ❌ Always throws on missing/bad values     | ✅ Returns provided fallback value                  |
| Works with complex POCOs          | ❌ Only scalar values                    | ✅ Supports binding to objects             | ✅ Supports binding to objects                      |
