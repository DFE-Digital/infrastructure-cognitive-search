using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.FilterExpressions;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.FilterExpressions.Context;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.FilterExpressions.Formatters;
using Xunit;

namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.Tests.Filtering.FilterExpressions;

public sealed class SearchInFilterExpressionTests
{
    [Fact]
    public void MultipleFacetValues_ReturnsFormattedInExpression()
    {
        // arrange
        SearchInFilterExpression filterExpression = new(new DefaultFilterExpressionFormatter());
        SearchFilterContext context = new("facet", ["value1", "value2" ]);
        const string expected = "search.in(facet, 'value1,value2')";

        // act
        var result = filterExpression.CreateFilterExpression(context);

        // assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void BoolFacetValues_Throws()
    {
        // arrange
        var filterExpression = new SearchInFilterExpression(
            new DefaultFilterExpressionFormatter());

        var context = new SearchFilterContext("facet", [true]);

        // act, assert
        Assert.Throws<ArgumentException>(() => filterExpression.CreateFilterExpression(context));
    }
}