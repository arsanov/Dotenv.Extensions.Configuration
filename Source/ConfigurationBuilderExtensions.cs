using Microsoft.Extensions.Configuration;

namespace Dotenv.Extensions.Configuration
{
    public static class ConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddDotenvFile(this IConfigurationBuilder builder, string dotenvFilePath, bool optional = false, string prefix = null)
        {
            if (File.Exists(dotenvFilePath))
            {
                return builder.Add(new DotenvConfigurationSource(dotenvFilePath, prefix));
            }
            else if (!optional)
            {
                throw new FileNotFoundException(dotenvFilePath);
            }
            else
            {
                return builder;
            }
        }
    }
}
