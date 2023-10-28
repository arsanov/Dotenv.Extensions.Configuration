using Microsoft.Extensions.Configuration;

namespace Dotenv
{
    public class DotenvConfigurationSource : FileConfigurationSource
    {
        public override IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            EnsureDefaults(builder);
            return new DotenvConfigurationProvider(this);
        }

        /// <summary>
        /// Prefix for environment variables to include
        /// </summary>
        public string Prefix { get; set; }
    }
}
