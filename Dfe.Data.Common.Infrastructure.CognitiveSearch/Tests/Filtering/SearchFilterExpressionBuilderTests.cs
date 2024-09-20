using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.FilterExpressions.Factories;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.LogicalOperators.Factories;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.Options;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.Tests.Filtering.FilterExpressions;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.Tests.Filtering.FilterExpressions.StubBuilders;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.Tests.Filtering.TestDoubles.StubBuilders;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.Tests.Search.TestDoubles;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Xunit;

namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.Tests.Filtering;

public class SearchFilterExpressionBuilderTests
{
    [Fact]
    public void BuildSearchFilterExpressions_WithValidSingleSearchFilterRequestWithMultipleParams_ReturnsExpectedAggregatedSearchFilter()
    {
        // arrange.
        ISearchFilterExpressionFactory searchFilterExpressionFactory =
            SearchFilterExpressionFactoryTestDouble.MockSearchFilterExpressionFactory();

        ILogicalOperatorFactory logicalOperatorFactory =
            LogicalOperatorFactoryTestDouble.MockLogicalOperatorFactory();

        FilterKeyToFilterExpressionMapOptions filterKeyToFilterExpressionMapOptions =
                new FilterKeyToFilterExpressionMapOptionsBuilder()
                    .WithFilterChainingLogicalOperator(filterChainingLogicalOperatorKey: "AndLogicalOperator")
                    .WithSearchFilterToExpressionMap(searchFilterToExpressionMap: new Dictionary<string, string>()
                    {
                        { "OFSTEDRATINGCODE", "SearchInFilterExpression"},
                        { "RELIGIOUSCHARACTERCODE", "SearchInFilterExpression" },
                        { "GEODISTANCE", "LessThanOrEqualToExpression" },
                        { "GEOLOCATION", "SearchGeoLocationFilterExpression" }
                    })
                    .Create();

        IOptions<FilterKeyToFilterExpressionMapOptions> options =
            IOptionsTestDouble.IOptionsMockFor(filterKeyToFilterExpressionMapOptions);

        SearchFilterExpressionsBuilder searchFilterExpressionBuilder = new(searchFilterExpressionFactory, logicalOperatorFactory, options);

        List<SearchFilterRequest> searchFilterRequests =
            SearchFilterRequestBuilder.Create().BuildSearchFilterRequestsWith(
               ("OFSTEDRATINGCODE", new List<object> { "The good", "The bad", "The ugly"}),
               ("RELIGIOUSCHARACTERCODE", new List<object> { "00", "02" })
            )
            .BuildSearchFilterRequests();

        // act.
        string searchFilterResult =
            searchFilterExpressionBuilder.BuildSearchFilterExpressions(searchFilterRequests);

        // assert.
        searchFilterResult.Should().NotBeNullOrWhiteSpace(searchFilterResult);
        searchFilterResult.Should().Be("search.in(OFSTEDRATINGCODE, 'The good,The bad,The ugly', ',') and search.in(RELIGIOUSCHARACTERCODE, '00,02', ',')");
    }

    [Fact]
    public void BuildSearchFilterExpressions_WithValidGeoLocationSearchFilterRequestWithMultipleParams_ReturnsExpectedAggregatedGeoLocationFilter()
    {
        // arrange.
        ISearchFilterExpressionFactory searchFilterExpressionFactory =
            SearchFilterExpressionFactoryTestDouble.MockSearchFilterExpressionFactory();

        ILogicalOperatorFactory logicalOperatorFactory =
            LogicalOperatorFactoryTestDouble.MockLogicalOperatorFactory();

        FilterKeyToFilterExpressionMapOptions filterKeyToFilterExpressionMapOptions =
                new FilterKeyToFilterExpressionMapOptionsBuilder()
                    .WithFilterChainingLogicalOperator(filterChainingLogicalOperatorKey: "AndLogicalOperator")
                    .WithSearchFilterToExpressionMap(searchFilterToExpressionMap: new Dictionary<string, string>()
                    {
                        { "OFSTEDRATINGCODE", "SearchInFilterExpression"},
                        { "RELIGIOUSCHARACTERCODE", "SearchInFilterExpression" },
                        { "GEODISTANCE", "LessThanOrEqualToExpression" },
                        { "GEOLOCATION", "SearchGeoLocationFilterExpression" }
                    })
                    .Create();

        IOptions<FilterKeyToFilterExpressionMapOptions> options =
            IOptionsTestDouble.IOptionsMockFor(filterKeyToFilterExpressionMapOptions);

        SearchFilterExpressionsBuilder searchFilterExpressionBuilder = new(searchFilterExpressionFactory, logicalOperatorFactory, options);

        List<SearchFilterRequest> searchFilterRequests =
            SearchFilterRequestBuilder.Create().BuildSearchFilterRequestsWith(
               ("GEOLOCATION", new List<object> { "-1.69469", "54.87835" }),
               ("GEODISTANCE", new List<object> { "4.8" })
            )
            .BuildSearchFilterRequests();

        // act.
        string searchFilterResult =
            searchFilterExpressionBuilder.BuildSearchFilterExpressions(searchFilterRequests);

        // assert.
        searchFilterResult.Should().NotBeNullOrWhiteSpace(searchFilterResult);
        searchFilterResult.Should().Be("geo.distance(Location,geography'POINT(-1.69469 54.87835)') and le 4.8");
    }
}
