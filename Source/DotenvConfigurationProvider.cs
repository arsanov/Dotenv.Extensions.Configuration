using Microsoft.Extensions.Configuration;
using DotNetEnv;

namespace Dotenv
{
    public class DotenvConfigurationProvider : FileConfigurationProvider
    {
        private readonly string prefix;

        public DotenvConfigurationProvider(DotenvConfigurationSource source) : base(source)
        {
            prefix = source.Prefix;
        }

        public override void Load(Stream stream)
        {
            var kvPairs = Env.Load(stream, new LoadOptions(false))
                .Where(pair => pair.Key.StartsWith(prefix))
                .Select(pair => new KeyValuePair<string, string>(pair.Key[prefix.Length..].Replace("__", ConfigurationPath.KeyDelimiter), pair.Value));

            var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var pair in kvPairs)
            {
                result[pair.Key] = pair.Value;
            }

            Data = result;
        }
    }
}
