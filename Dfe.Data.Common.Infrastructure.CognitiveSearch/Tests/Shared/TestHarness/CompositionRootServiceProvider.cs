using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.Tests.Shared.TestHarness
{
    public sealed class CompositionRootServiceProvider : IDisposable
    {
        private IServiceCollection? _services;
        private IConfiguration? _config;

        public CompositionRootServiceProvider InitialiseServiceCollection(IConfiguration config)
        {
            _config = config;
            _services?.Clear();
            _services = new ServiceCollection();
            _services.AddSingleton(config);
            return this;
        }

        public CompositionRootServiceProvider WithAzureSearchServices()
        {
            _services?.AddAzureSearchServices(_config!);
            return this;
        }

        public CompositionRootServiceProvider WithAzureGeoLocationSearchServices()
        {
            _services?.AddAzureGeoLocationSearchServices(_config!);
            return this;
        }

        public CompositionRootServiceProvider WithAzureSearchFilterServices()
        {
            _services?.AddAzureSearchFilterServices(_config!);
            return this;
        }

        public CompositionRootServiceProvider WithReplacementService<TService>(TService service) where TService : class
        {
            _services?.RemoveAll<TService>();
            _services?.AddScoped(_ => service);

            return this;
        }

        public IServiceProvider? Create() => _services?.BuildServiceProvider();

        public void Dispose()
        {
            _services?.Clear();
            _services = null!;
        }
    }
}