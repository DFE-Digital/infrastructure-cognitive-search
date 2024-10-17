using Microsoft.Extensions.Configuration;

namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.Tests.Shared.TestHarness
{
    public sealed class ConfigBuilder
    {
        public IConfiguration SetupConfiguration(Dictionary<string, string?> options)
        {
            ConfigurationBuilder configBuilder = new();
            configBuilder.AddInMemoryCollection(options);
            return configBuilder.Build();
        }
    }
}