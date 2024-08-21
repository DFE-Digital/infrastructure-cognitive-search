using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.FilterExpressions.Context;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.FilterExpressions.Factories;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.LogicalOperators.Factories;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.Tests.Filtering.FilterExpressions;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.Tests.Filtering.FilterExpressions.StubBuilders;
using FluentAssertions;
using Xunit;

namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.Tests.Filtering;

public class SearchFilterExpressionBuilderTests
{
    [Fact]
    public void AggregateSearchExpressions_WithValidSingleSearchFilterContextWithMultipleParams_ReturnsExpectedAggregatedSearchFilter()
    {
        // arrange.
        ISearchFilterExpressionFactory searchFilterExpressionFactory =
            SearchFilterExpressionFactoryTestDouble.MockSearchFilterExpressionFactory();

        ILogicalOperatorFactory logicalOperatorFactory =
            LogicalOperatorFactoryTestDouble.MockLogicalOperatorFactory();

        SearchFilterExpressionBuilder searchFilterExpressionBuilder = new(searchFilterExpressionFactory, logicalOperatorFactory);

        List<SearchFilterContext> searchFilterContexts =
            SearchFilterContextBuilder.Create().BuildSearchFilterContextsWith(
               ("OFSTEDRATINGCODE", new List<object> { "2", "5", "9", "12" }),
               ("RELIGIOUSCHARACTERCODE", new List<object> { "00", "02" })
            )
            .BuildSearchFilterContexts();

        // act.
        string searchFilterResult =
            searchFilterExpressionBuilder.BuildSearchFilter(searchFilterContexts);

        // assert.
        searchFilterResult.Should().NotBeNullOrWhiteSpace(searchFilterResult);
        searchFilterResult.Should().Be("search.in(OFSTEDRATINGCODE, '2,5,9,12') and search.in(RELIGIOUSCHARACTERCODE, '00,02')");
    }
}
