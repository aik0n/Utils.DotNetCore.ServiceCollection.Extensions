using Bogus;
using Microsoft.Extensions.Configuration;
using Utils.DotNetCore.IServiceCollection;

namespace Tests.Unit
{
    public class ExtensionsTests
    {
        private readonly Faker _faker = new();

        [Fact]
        public void GetConfigurationValue_ShouldReturnValue_WhenKeyExists()
        {
            var key = "AppSettings:ApiKey";
            var expected = _faker.Random.Guid().ToString();

            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    [key] = expected
                })
                .Build();

            var result = config.GetConfigurationValue<string>("AppSettings", "ApiKey");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetConfigurationValue_ShouldThrowKeyNotFound_WhenKeyDoesNotExist()
        {
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>())
                .Build();

            Assert.Throws<KeyNotFoundException>(() => config.GetConfigurationValue<string>("Missing", "Key"));
        }

        [Fact]
        public void GetConfigurationValue_ShouldThrowInvalidOperation_WhenValueIsNull()
        {
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["App:Value"] = null
                })
                .Build();

            Assert.Throws<KeyNotFoundException>(() => config.GetConfigurationValue<int>("App", "Value"));
        }

        [Fact]
        public void GetConfigurationValueOrDefault_ShouldReturnValue_WhenKeyExists()
        {
            var key = "User:Name";
            var expected = _faker.Person.FullName;

            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    [key] = expected
                })
                .Build();

            var result = config.GetConfigurationValueOrDefault("default-name", "User", "Name");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetConfigurationValueOrDefault_ShouldReturnDefault_WhenKeyDoesNotExist()
        {
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["randomKey"] = "randomValue"
                })
                .Build();

            var defaultValue = "fallback";
            var result = config.GetConfigurationValueOrDefault(defaultValue, "Does", "NotExist");

            Assert.Equal(defaultValue, result);
        }
    }
}