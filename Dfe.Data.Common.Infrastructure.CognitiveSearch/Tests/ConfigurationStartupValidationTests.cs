using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByGeoLocation;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword.Providers;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.Tests.Shared.TestHarness;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.Tests
{
    public sealed class ConfigurationStartupValidationTests :  IClassFixture<ConfigBuilder>, IClassFixture<CompositionRootServiceProvider>
    {

        private readonly ConfigBuilder _configBuilder;
        private readonly CompositionRootServiceProvider _compositionRootServiceProvider;

        public ConfigurationStartupValidationTests(ConfigBuilder configBuilder, CompositionRootServiceProvider serviceProvider)
        {
            _configBuilder = configBuilder;
            _compositionRootServiceProvider = serviceProvider;
        }

        [Fact]
        public void AddAzureSearchServices_MissingAzureSearchConnectionOptions_Credentials_ThrowOptionsValidationException()
        {
            Dictionary<string, string?> config = new() {
                {"AzureSearchConnectionOptions:EndpointUri", "https://test-search-service-uri"}
            };

            IConfiguration configuration =
                _configBuilder.SetupConfiguration(config);

            IServiceProvider? serviceProvider =
                _compositionRootServiceProvider?
                    .InitialiseServiceCollection(configuration)
                    .WithAzureSearchServices()
                    .Create();

            // act, assert
            OptionsValidationException exception =
                 Assert.Throws<OptionsValidationException>(() =>
                     serviceProvider?.GetRequiredService<ISearchByKeywordClientProvider>());

            Assert.Equal("DataAnnotation validation failed for 'AzureSearchConnectionOptions' members: 'Credentials' with the error: 'The Credentials field is required.'.", exception.Message);
        }

        [Fact]
        public void AddAzureSearchServices_MissingAzureSearchConnectionOptions_EndpointUri_ThrowOptionsValidationException()
        {
            Dictionary<string, string?> config = new() {
                {"AzureSearchConnectionOptions:Credentials", "credentials" }
            };

            IConfiguration configuration =
                _configBuilder.SetupConfiguration(config);

            IServiceProvider? serviceProvider =
                _compositionRootServiceProvider?
                    .InitialiseServiceCollection(configuration)
                    .WithAzureSearchServices()
                    .Create();

            // act, assert
            OptionsValidationException exception =
                 Assert.Throws<OptionsValidationException>(() =>
                     serviceProvider?.GetRequiredService<ISearchByKeywordClientProvider>());

            Assert.Equal("DataAnnotation validation failed for 'AzureSearchConnectionOptions' members: 'EndpointUri' with the error: 'The EndpointUri field is required.'.", exception.Message);
        }

        [Fact]
        public void AddAzureGeoLocationSearchServices_MissingGeoLocationOptions_MapsServiceUri_ThrowOptionsValidationException()
        {
            Dictionary<string, string?> config = new() {
                {"GeoLocationOptions:SearchEndpoint", "Some/Test/Endpoint"},
                {"GeoLocationOptions:MapsSubscriptionKey", "subscription-key"}
            };

            IConfiguration configuration =
                _configBuilder.SetupConfiguration(config);

            IServiceProvider? serviceProvider =
                _compositionRootServiceProvider?
                    .InitialiseServiceCollection(configuration)
                    .WithAzureGeoLocationSearchServices()
                    .Create();

            // act, assert
            OptionsValidationException exception =
                 Assert.Throws<OptionsValidationException>(() =>
                     serviceProvider?.GetRequiredService<IGeoLocationService>());

            Assert.Equal("DataAnnotation validation failed for 'GeoLocationOptions' members: 'MapsServiceUri' with the error: 'The MapsServiceUri field is required.'.", exception.Message);
        }

        [Fact]
        public void AddAzureGeoLocationSearchServices_MissingGeoLocationOptions_SearchEndpoint_ThrowOptionsValidationException()
        {
            Dictionary<string, string?> config = new() {
                {"GeoLocationOptions:MapsServiceUri", "http://test-geo-location-service-uri" },
                {"GeoLocationOptions:MapsSubscriptionKey", "subscription-key"}
            };

            IConfiguration configuration =
                _configBuilder.SetupConfiguration(config);

            IServiceProvider? serviceProvider =
                _compositionRootServiceProvider?
                    .InitialiseServiceCollection(configuration)
                    .WithAzureGeoLocationSearchServices()
                    .Create();

            // act, assert
            OptionsValidationException exception =
                 Assert.Throws<OptionsValidationException>(() =>
                     serviceProvider?.GetRequiredService<IGeoLocationService>());

            exception.Message.Should().Be("DataAnnotation validation failed for 'GeoLocationOptions' members: 'SearchEndpoint' with the error: 'The SearchEndpoint field is required.'.");
        }

        [Fact]
        public void AddAzureGeoLocationSearchServices_MissingGeoLocationOptions_MapsSubscriptionKey_ThrowOptionsValidationException()
        {
            Dictionary<string, string?> config = new() {
                {"GeoLocationOptions:MapsServiceUri", "http://test-geo-location-service-uri" },
                {"GeoLocationOptions:SearchEndpoint", "Some/Test/Endpoint"}
            };

            IConfiguration configuration =
                _configBuilder.SetupConfiguration(config);

            IServiceProvider? serviceProvider =
                _compositionRootServiceProvider?
                    .InitialiseServiceCollection(configuration)
                    .WithAzureGeoLocationSearchServices()
                    .Create();

            // act, assert
            OptionsValidationException exception =
                 Assert.Throws<OptionsValidationException>(() =>
                     serviceProvider?.GetRequiredService<IGeoLocationService>());

            exception.Message.Should().Be(
                "DataAnnotation validation failed for 'GeoLocationOptions' members: 'MapsSubscriptionKey' with the error: 'The MapsSubscriptionKey field is required.'.");
        }

        [Fact]
        public void AddAzureSearchFilterServices_MissingFilterKeyToFilterExpressionMapOptions_FilterChainingLogicalOperator_ThrowOptionsValidationException()
        {
            Dictionary<string, string?> config = new() {
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

            // act, assert
            OptionsValidationException exception =
                 Assert.Throws<OptionsValidationException>(() =>
                     serviceProvider?.GetRequiredService<ISearchFilterExpressionsBuilder>());

            exception.Message.Should().Be(
                "DataAnnotation validation failed for 'FilterKeyToFilterExpressionMapOptions' members: 'FilterChainingLogicalOperator' with the error: 'The FilterChainingLogicalOperator field is required.'.");
        }

        [Fact]
        public void AddAzureSearchFilterServices_MissingFilterKeyToFilterExpressionMapOptions_SearchFilterToExpressionMap_ThrowOptionsValidationException()
        {
            Dictionary<string, string?> config = new() {
               { "FilterKeyToFilterExpressionMapOptions:FilterChainingLogicalOperator", "AndLogicalOperator" }
            };

            IConfiguration configuration =
                _configBuilder.SetupConfiguration(config);

            IServiceProvider? serviceProvider =
                _compositionRootServiceProvider?
                    .InitialiseServiceCollection(configuration)
                    .WithAzureSearchFilterServices()
                    .Create();

            // act, assert
            OptionsValidationException exception =
                 Assert.Throws<OptionsValidationException>(() =>
                     serviceProvider?.GetRequiredService<ISearchFilterExpressionsBuilder>());

            exception.Message.Should().Be(
                "DataAnnotation validation failed for 'FilterKeyToFilterExpressionMapOptions' members: 'SearchFilterToExpressionMap' with the error: 'The field SearchFilterToExpressionMap must be a string or array type with a minimum length of '1'.'.");
        }
    }
}
