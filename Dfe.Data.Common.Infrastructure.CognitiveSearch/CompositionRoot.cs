using Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByGeoLocation;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByGeoLocation.Options;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByGeoLocation.Providers;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword.Options;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Dfe.Data.Common.Infrastructure.CognitiveSearch;

/// <summary>
/// The composition root provides a unified location in the application where the composition
/// of the object graphs for the application take place, using the dependency injection (IOC).
/// </summary>
public static class CompositionRoot
{
    /// <summary>
    /// Extension method which provides all the pre-registrations required to
    /// access azure search services, and perform searches across provisioned indexes.
    /// </summary>
    /// <param name="services">
    /// The originating application services onto which to register the search dependencies.
    /// </param>
    /// <param name="configuration">
    /// The originating configuration block from which to derive search service settings.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// The exception thrown if no valid T:Microsoft.Extensions.DependencyInjection.IServiceCollection
    /// is provisioned.
    /// </exception>
    public static void AddDefaultCognitiveSearchServices(this IServiceCollection services, IConfiguration configuration)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services),
                "A service collection is required to configure the azure cognitive search dependencies.");
        }

        services.TryAddSingleton<ISearchByKeywordClientProvider, SearchByKeywordClientProvider>();
        services.TryAddSingleton<ISearchIndexNamesProvider, SearchIndexNamesProvider>();
        services.TryAddSingleton<ISearchByKeywordService, DefaultSearchByKeywordService>();
        services.TryAddScoped<IGeoLocationClientProvider, GeoLocationClientProvider>();
        services.TryAddScoped<IGeoLocationService, DefaultGeoLocationService>();

        services.AddOptions<AzureSearchConnectionOptions>()
           .Configure<IConfiguration>(
               (settings, configuration) =>
                   configuration
                       .GetSection(nameof(AzureSearchConnectionOptions))
                       .Bind(settings));

        services.AddOptions<GeoLocationOptions>()
           .Configure<IConfiguration>(
               (settings, configuration) =>
                   configuration
                       .GetSection(nameof(GeoLocationOptions))
                       .Bind(settings));

        services.AddHttpClient("GeoLocationHttpClient", config =>
        {
            var geoLocationOptions =
                configuration
                    .GetSection(nameof(GeoLocationOptions)).Get<GeoLocationOptions>();

            ArgumentNullException.ThrowIfNull(geoLocationOptions);
            ArgumentNullException.ThrowIfNullOrWhiteSpace(geoLocationOptions.MapsServiceUri);

            config.BaseAddress = new Uri(geoLocationOptions.MapsServiceUri);
            config.Timeout =
                new TimeSpan(
                    geoLocationOptions.RequestTimeOutHours,
                    geoLocationOptions.RequestTimeOutMinutes,
                    geoLocationOptions.RequestTimeOutSeconds);

            config.DefaultRequestHeaders.Clear();
        });
    }
}
