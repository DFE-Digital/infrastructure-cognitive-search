using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.FilterExpressions;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.FilterExpressions.Formatters;
using Xunit;

namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.Tests.Filtering.FilterExpressions;

public sealed class SearchInFilterExpressionTests
{
    [Fact]
    public void GetFilterExpression_MultipleFacetValues_ReturnsFormattedInExpression()
    {
        // arrange
        SearchInFilterExpression filterExpression = new(new DefaultFilterExpressionFormatter());
        SearchFilterRequest context = new("facet", ["value1", "value2", "value3"]);

        const string expected = "search.in(facet, 'value1,value2,value3')";

        // act
        var result = filterExpression.GetFilterExpression(context);

        // assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetFilterExpression_BoolFacetValues_ThrowsArgumentException()
    {
        // arrange
        var filterExpression = new SearchInFilterExpression(
            new DefaultFilterExpressionFormatter());

        var context = new SearchFilterRequest("facet", [true]);

        // act, assert
        Assert.Throws<ArgumentException>(() => filterExpression.GetFilterExpression(context));
    }

    [Fact]
    public void Ctor_NullFilterExpressionFormatter_ThrowsArgumentException()
    {
        // arrange/act.
        Action failedCtorAction = () =>
            new SearchInFilterExpression(filterExpressionFormatter: null!);

        //assert
        ArgumentException exception =
            Assert.Throws<ArgumentNullException>(failedCtorAction);
        Assert.Equal("Value cannot be null. (Parameter 'filterExpressionFormatter')", exception.Message);
    }
}