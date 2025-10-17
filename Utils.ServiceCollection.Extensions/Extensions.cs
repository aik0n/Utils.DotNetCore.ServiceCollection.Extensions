using Microsoft.Extensions.Configuration;

namespace Utils.DotNetCore.IServiceCollection
{
    public static class Extensions
    {
        public static T GetConfigurationValue<T>(this IConfiguration configuration, params string[] keys)
        {
            var section = configuration;

            foreach (var key in keys)
            {
                if (false == section.GetSection(key).Exists())
                {
                    throw new KeyNotFoundException($"Configuration key '{string.Join(":", keys)}' was not found.");
                }

                section = section.GetSection(key);
            }

            var value = section.Get<T>();

            if (value is null)
            {
                throw new InvalidOperationException($"Configuration key '{string.Join(":", keys)}' could not be bound to type {typeof(T).Name}.");
            }

            return value;
        }

        public static T GetConfigurationValueOrDefault<T>(this IConfiguration configuration, T defaultValue, params string[] keys)
        {
            var section = configuration;

            foreach (var key in keys)
            {
                if (false == section.GetSection(key).Exists())
                {
                    return defaultValue;
                }

                section = section.GetSection(key);
            }

            var value = section.Get<T>();

            if (value is null)
            {
                return defaultValue;
            }

            return value;
        }
    }
}