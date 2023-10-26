using Microsoft.Extensions.Configuration;
using DotNetEnv;

namespace Dotenv
{
    public class DotenvConfigurationProvider : ConfigurationProvider
    {
        private readonly string dotenvFilePath;
        private readonly string prefix;

        public DotenvConfigurationProvider(string dotenvFilePath, string prefix)
        {
            this.dotenvFilePath = dotenvFilePath;
            this.prefix = prefix ?? String.Empty;
        }

        public override void Load()
        {
            var kvPairs = Env.Load(dotenvFilePath, new LoadOptions(false))
                .Where(pair => pair.Key.StartsWith(prefix))
                .Select(pair => new KeyValuePair<string, string>(pair.Key[prefix.Length..].Replace("__", ConfigurationPath.KeyDelimiter), pair.Value));

            var result = new Dictionary<string, string>();
            foreach (var pair in kvPairs)
            {
                result[pair.Key] = pair.Value;
            }

            Data = result;
        }
    }
}
