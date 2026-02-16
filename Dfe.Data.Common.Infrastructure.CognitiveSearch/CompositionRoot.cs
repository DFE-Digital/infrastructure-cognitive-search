using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.FilterExpressions;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.FilterExpressions.Factories;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.FilterExpressions.Formatters;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.LogicalOperators;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.LogicalOperators.Factories;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.Options;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByGeoLocation;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByGeoLocation.Options;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByGeoLocation.Providers;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword.IndexNames.Options;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword.IndexNames.Providers;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword.Options;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword.Providers;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword.Transformer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Dfe.Data.Common.Infrastructure.CognitiveSearch;

/// <summary>
/// The composition root provides a unified location in the application where the composition
/// of the object graphs for the application take place, using the IOC container.
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
    public static void AddAzureSearchServices(this IServiceCollection services, IConfiguration configuration)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services),
                "A service collection is required to configure the azure cognitive search dependencies.");
        }

        services.TryAddSingleton<ISearchByKeywordClientProvider, SearchByKeywordClientProvider>();
        services.TryAddSingleton<ISearchIndexNamesProvider, AzureRemoteSearchIndexNamesProvider>();
        services.TryAddSingleton<ISearchByKeywordService, DefaultSearchByKeywordService>();
        services.TryAddSingleton<ISearchKeywordTransformer, PartialWordMatchSearchKeywordTransformer>();

        services.AddOptions<AzureSearchConnectionOptions>()
            .Bind(configuration.GetSection(nameof(AzureSearchConnectionOptions)))
            .ValidateDataAnnotations()
            .ValidateOnStart();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddAzureIndexNamesConfigurationProvider(this IServiceCollection services, IConfiguration configuration)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services),
                "A service collection is required to configure the azure cognitive search dependencies.");
        }

        services.RemoveAll<ISearchIndexNamesProvider>();

        services
            .AddOptions<SearchIndexNamesOptions>()
            .Bind(configuration.GetSection(nameof(SearchIndexNamesOptions)));

        services.AddSingleton(
            (sp) =>
                sp.GetRequiredService<IOptions<SearchIndexNamesOptions>>().Value);

        services.TryAddSingleton<ISearchIndexNamesProvider, AzureConfigurationSearchIndexNamesProvider>();

        return services;
    }

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
    public static void AddAzureGeoLocationSearchServices(this IServiceCollection services, IConfiguration configuration)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services),
                "A service collection is required to configure the geo-location search dependencies.");
        }

        services.TryAddScoped<IGeoLocationClientProvider, GeoLocationClientProvider>();
        services.TryAddScoped<IGeoLocationService, DefaultGeoLocationService>();

        services.AddOptions<GeoLocationOptions>()
            .Bind(configuration.GetSection(nameof(GeoLocationOptions)))
            .ValidateDataAnnotations()
            .ValidateOnStart();

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

    /// <summary>
    /// Extension method which provides all the pre-registrations required to
    /// access azure search filter services, and perform filtered searches across provisioned indexes.
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
    public static void AddAzureSearchFilterServices(this IServiceCollection services, IConfiguration configuration)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services),
                "A service collection is required to configure the azure cognitive search filter dependencies.");
        }

        services.TryAddScoped<IFilterExpressionFormatter, DefaultFilterExpressionFormatter>();
        services.TryAddScoped<AndLogicalOperator>();
        services.TryAddScoped<OrLogicalOperator>();
        services.TryAddScoped<SearchInFilterExpression>();
        services.TryAddScoped<SearchGeoLocationFilterExpression>();
        services.TryAddScoped<LessThanOrEqualToExpression>();
        services.TryAddScoped<ISearchFilterExpressionsBuilder, SearchFilterExpressionsBuilder>();

        services.TryAddSingleton<ISearchFilterExpressionFactory>(provider =>
        {
            var scopedSearchFilterExpressionProvider = provider.CreateScope();
            var searchFilterExpressions =
                new Dictionary<string, Func<ISearchFilterExpression>>()
                {
                    ["SearchInFilterExpression"] = () =>
                        scopedSearchFilterExpressionProvider
                            .ServiceProvider.GetRequiredService<SearchInFilterExpression>(),
                    ["LessThanOrEqualToExpression"] = () =>
                        scopedSearchFilterExpressionProvider
                            .ServiceProvider.GetRequiredService<LessThanOrEqualToExpression>(),
                    ["SearchGeoLocationFilterExpression"] = () =>
                        scopedSearchFilterExpressionProvider.
                            ServiceProvider.GetRequiredService<SearchGeoLocationFilterExpression>()
                };

            return new SearchFilterExpressionFactory(searchFilterExpressions);
        });

        services.TryAddSingleton<ILogicalOperatorFactory>(provider =>
        {
            var scopedLogicalOperatorExpressionProvider = provider.CreateScope();
            var logicalOperators =
                new Dictionary<string, Func<ILogicalOperator>>()
                {
                    ["AndLogicalOperator"] = () =>
                           scopedLogicalOperatorExpressionProvider
                               .ServiceProvider.GetRequiredService<AndLogicalOperator>(),
                    ["OrLogicalOperator"] = () =>
                        scopedLogicalOperatorExpressionProvider
                            .ServiceProvider.GetRequiredService<OrLogicalOperator>()
                };

            return new LogicalOperatorFactory(logicalOperators);
        });

        services.AddOptions<FilterKeyToFilterExpressionMapOptions>()
            .Bind(configuration.GetSection(nameof(FilterKeyToFilterExpressionMapOptions)))
            .ValidateDataAnnotations()
            .ValidateOnStart();
    }
}
