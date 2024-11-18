using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByGeoLocation;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByGeoLocation.Model;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByGeoLocation.Providers;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword.Providers;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.Tests.SearchByGeoLocation.TestDoubles;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.Tests.SearchByKeyword.TestDoubles;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.Tests.Shared.TestHarness;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Dynamic;
using System.Net;
using Xunit;

namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.Tests
{
    public sealed class CompositionRootTests : IClassFixture<ConfigBuilder>, IClassFixture<CompositionRootServiceProvider>
    {
        private readonly ConfigBuilder _configBuilder;
        private readonly CompositionRootServiceProvider _compositionRootServiceProvider;

        public CompositionRootTests(ConfigBuilder configBuilder, CompositionRootServiceProvider serviceProvider)
        {
            _configBuilder = configBuilder;
            _compositionRootServiceProvider = serviceProvider;
        }

        [Fact]
        public async Task AddAzureSearchServices_RegistersAllDependencies()
        {
            Dictionary<string, string?> config = new() {
                {"AzureSearchConnectionOptions:Credentials", "credentials" },
                {"AzureSearchConnectionOptions:EndpointUri", "https://test-search-service-uri"}
            };

            IConfiguration configuration =
                _configBuilder.SetupConfiguration(config);

            ISearchByKeywordClientProvider mockSearchByKeywordClientProvider =
                SearchByKeywordClientProviderTestDouble.CreateWithDefaultResponse();

            IServiceProvider? serviceProvider =
                _compositionRootServiceProvider?
                    .InitialiseServiceCollection(configuration)
                    .WithAzureSearchServices()
                    .WithReplacementService(mockSearchByKeywordClientProvider)
                .Create();

            ISearchByKeywordService? searchByKeywordService =
                serviceProvider?.GetRequiredService<ISearchByKeywordService>();

            // act
            var response =
                await searchByKeywordService!
                    .SearchAsync<ExpandoObject>(
                        searchKeyword: "Test",
                        searchIndex: "Index",
                        searchOptions: new Azure.Search.Documents.SearchOptions()
                    )!;

            response.Should().NotBeNull();
        }

        [Fact]
        public async Task AddAzureGeoLocationSearchServices_RegistersAllDependencies()
        {
            Dictionary<string, string?> config = new() {
                {"GeoLocationOptions:MapsServiceUri", "http://test-geo-location-service-uri" },
                {"GeoLocationOptions:SearchEndpoint", "Some/Test/Endpoint"},
                {"GeoLocationOptions:MapsSubscriptionKey", "subscription-key"},
                {"GeoLocationOptions:RequestTimeOutHours", "12"}
            };

            IConfiguration configuration =
                _configBuilder.SetupConfiguration(config);

            IGeoLocationClientProvider mockGeoLocationClientProvider =
                GeoLocationClientProviderTestDouble
                    .CreateWithHttpStatusAndResponse(HttpStatusCode.OK, "{ }");

            IServiceProvider? serviceProvider =
                _compositionRootServiceProvider?
                    .InitialiseServiceCollection(configuration)
                    .WithAzureGeoLocationSearchServices()
                    .WithReplacementService(mockGeoLocationClientProvider)
                    .Create();

            IGeoLocationService? geoLocationService =
                serviceProvider?.GetRequiredService<IGeoLocationService>();

            // act
            GeoLocationServiceResponse? response =
                await geoLocationService?.SearchGeoLocationAsync("TE5T 0NE")!;

            response?.Should().NotBeNull();
        }

        [Fact]
        public void AddAzureSearchFilterServices_RegistersAllDependencies()
        {
            Dictionary<string, string?> config = new() {
                { "FilterKeyToFilterExpressionMapOptions:FilterChainingLogicalOperator", "AndLogicalOperator" },
                { "FilterKeyToFilterExpressionMapOptions:SearchFilterToExpressionMap:test-key:FilterExpressionKey", "SearchInFilterExpression" },
                { "FilterKeyToFilterExpressionMapOptions:SearchFilterToExpressionMap:test-key:FilterExpressionValuesDelimiter", "¬" }
            };

            IConfiguration configuration =
                _configBuilder.SetupConfiguration(config);

            IServiceProvider? serviceProvider =
                _compositionRootServiceProvider?
                    .InitialiseServiceCollection(configuration)
                    .WithAzureSearchFilterServices()
                    .Create();

            ISearchFilterExpressionsBuilder? searchFilterExpressionsBuilder =
                serviceProvider?.GetRequiredService<ISearchFilterExpressionsBuilder>();

            // act
            IEnumerable<SearchFilterRequest> searchFilterRequests =
                [
                    new SearchFilterRequest("test-key", ["test-value-1"])
                ];

            string? response = searchFilterExpressionsBuilder?.BuildSearchFilterExpressions(searchFilterRequests);

            response?.Should().NotBeNull().And.Be("search.in(test-key, 'test-value-1', '¬')");
        }
    }
}
