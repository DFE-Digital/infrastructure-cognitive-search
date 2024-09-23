using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.FilterExpressions;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.FilterExpressions.Formatters;
using Xunit;

namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.Tests.Filtering.FilterExpressions;

public sealed class SearchInFilterExpressionTests
{
    [Fact]
    public void GetFilterExpression_MultipleFilterValuesAndDelimiterSpecified_ReturnsFormattedInExpression()
    {
        // arrange
        SearchInFilterExpression filterExpression = new(new DefaultFilterExpressionFormatter());
        SearchFilterRequest request = new("filter", ["value 1", "value    2", "value3"]);
        request.SetFilterValuesDelimiter("$");

        const string expected = "search.in(filter, 'value 1$value    2$value3', '$')";

        // act
        string result = filterExpression.GetFilterExpression(request);

        // assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetFilterExpression_BoolFilterValuesAndDelimiterSpecified_ThrowsArgumentException()
    {
        // arrange
        SearchInFilterExpression filterExpression = new SearchInFilterExpression(
            new DefaultFilterExpressionFormatter());

        SearchFilterRequest request = new("filter", [true]);
        request.SetFilterValuesDelimiter(",");

        // act, assert
        Assert.Throws<ArgumentException>(() => filterExpression.GetFilterExpression(request));
    }

    [Fact]
    public void GetFilterExpression_MultipleFilterValuesAndDelimiterNotSpecifiedWhenRequired_AppliesDefaultCommaSeparatedDelimiter()
    {
        // arrange
        SearchInFilterExpression filterExpression = new(new DefaultFilterExpressionFormatter());
        SearchFilterRequest request = new("filter", ["value 1", "value    2", "value3"]);

        const string expected = "search.in(filter, 'value 1,value    2,value3', ',')";

        // act.
        string result = filterExpression.GetFilterExpression(request);

        // assert
        Assert.Equal(expected, result);
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