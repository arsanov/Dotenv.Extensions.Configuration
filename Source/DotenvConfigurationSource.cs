using Microsoft.Extensions.Configuration;

namespace Dotenv
{
    public class DotenvConfigurationSource : IConfigurationSource
    {
        private readonly string dotenvFilePath;
        private readonly string prefix;

        public DotenvConfigurationSource(string dotenvFilePath, string prefix)
        {
            this.dotenvFilePath = dotenvFilePath;
            this.prefix = prefix;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new DotenvConfigurationProvider(dotenvFilePath, prefix);
        }
    }
}
