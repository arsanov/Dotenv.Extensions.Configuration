using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;

namespace Dotenv.Extensions.Configuration
{
    public static class ConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddDotenvFile(this IConfigurationBuilder builder, string dotenvFilePath) => AddDotenvFile(builder, dotenvFilePath, false);
        public static IConfigurationBuilder AddDotenvFile(this IConfigurationBuilder builder, string dotenvFilePath, string prefix) => AddDotenvFile(builder, dotenvFilePath, optional: false, prefix);
        public static IConfigurationBuilder AddDotenvFile(this IConfigurationBuilder builder, string dotenvFilePath, bool optional) => AddDotenvFile(builder, dotenvFilePath, optional, String.Empty);
        public static IConfigurationBuilder AddDotenvFile(this IConfigurationBuilder builder, string dotenvFilePath, bool optional, string prefix) => AddDotenvFile(builder, dotenvFilePath, optional, prefix, false);
        public static IConfigurationBuilder AddDotenvFile(this IConfigurationBuilder builder, string dotenvFilePath, bool optional, string prefix, bool reloadOnChange) => AddDotenvFile(builder, dotenvFilePath, optional, prefix, reloadOnChange, null);

        public static IConfigurationBuilder AddDotenvFile(this IConfigurationBuilder builder, string dotenvFilePath, bool optional, string prefix, bool reloadOnChange, IFileProvider provider)
        {
            return builder.AddDotenvFile(s =>
            {
                s.FileProvider = provider;
                s.Path = dotenvFilePath;
                s.Optional = optional;
                s.ReloadOnChange = reloadOnChange;
                s.Prefix = prefix;
                s.ResolveFileProvider();
            });
        }

        public static IConfigurationBuilder AddDotenvFile(this IConfigurationBuilder builder, Action<DotenvConfigurationSource> configureSource)
            => builder.Add(configureSource);
    }
}
