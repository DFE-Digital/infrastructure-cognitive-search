using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.FilterExpressions.Context;
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
    public void BuildSearchFilterExpressions_WithValidSingleSearchFilterContextWithMultipleParams_ReturnsExpectedAggregatedSearchFilter()
    {
        // arrange.
        ISearchFilterExpressionFactory searchFilterExpressionFactory =
            SearchFilterExpressionFactoryTestDouble.MockSearchFilterExpressionFactory();

        ILogicalOperatorFactory logicalOperatorFactory =
            LogicalOperatorFactoryTestDouble.MockLogicalOperatorFactory();

        FilterKeyToFilterExpressionMapOptions filterKeyToFilterExpressionMapOptions =
                new FilterKeyToFilterExpressionMapOptionsBuilder()
                    .WithDefaultLogicalOperator(defaultLogicalOperatorKey: "AndLogicalOperator")
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

        List<SearchFilterContext> searchFilterContexts =
            SearchFilterContextBuilder.Create().BuildSearchFilterContextsWith(
               ("OFSTEDRATINGCODE", new List<object> { "2", "5", "9", "12" }),
               ("RELIGIOUSCHARACTERCODE", new List<object> { "00", "02" })
            )
            .BuildSearchFilterContexts();

        // act.
        string searchFilterResult =
            searchFilterExpressionBuilder.BuildSearchFilterExpressions(searchFilterContexts);

        // assert.
        searchFilterResult.Should().NotBeNullOrWhiteSpace(searchFilterResult);
        searchFilterResult.Should().Be("search.in(OFSTEDRATINGCODE, '2,5,9,12') and search.in(RELIGIOUSCHARACTERCODE, '00,02')");
    }

    [Fact]
    public void BuildSearchFilterExpressions_WithValidGeoLocationSearchFilterContextWithMultipleParams_ReturnsExpectedAggregatedGeoLocationFilter()
    {
        // arrange.
        ISearchFilterExpressionFactory searchFilterExpressionFactory =
            SearchFilterExpressionFactoryTestDouble.MockSearchFilterExpressionFactory();

        ILogicalOperatorFactory logicalOperatorFactory =
            LogicalOperatorFactoryTestDouble.MockLogicalOperatorFactory();

        FilterKeyToFilterExpressionMapOptions filterKeyToFilterExpressionMapOptions =
                new FilterKeyToFilterExpressionMapOptionsBuilder()
                    .WithDefaultLogicalOperator(defaultLogicalOperatorKey: "AndLogicalOperator")
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

        List<SearchFilterContext> searchFilterContexts =
            SearchFilterContextBuilder.Create().BuildSearchFilterContextsWith(
               ("GEOLOCATION", new List<object> { "-1.69469", "54.87835" }),
               ("GEODISTANCE", new List<object> { "4.8" })
            )
            .BuildSearchFilterContexts();

        // act.
        string searchFilterResult =
            searchFilterExpressionBuilder.BuildSearchFilterExpressions(searchFilterContexts);

        // assert.
        searchFilterResult.Should().NotBeNullOrWhiteSpace(searchFilterResult);
        searchFilterResult.Should().Be("geo.distance(Location,geography'POINT(-1.69469 54.87835)') and le 4.8");
    }
}
