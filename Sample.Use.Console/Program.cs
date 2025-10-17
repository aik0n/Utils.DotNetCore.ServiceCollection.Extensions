using Utils.DotNetCore.IServiceCollection;

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