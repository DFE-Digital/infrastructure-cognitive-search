using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.FilterExpressions;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.FilterExpressions.Context;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.FilterExpressions.Formatters;
using Xunit;

namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.Tests.Filtering.FilterExpressions;

public sealed class LessThanOrEqualToExpressionTests
{
    [Fact]
    public void GetFilterExpression_SingleFacetValue_ReturnsFormattedLessThanOrEqualToExpression()
    {
        // arrange
        LessThanOrEqualToExpression filterExpression = new(new DefaultFilterExpressionFormatter());
        SearchFilterRequest context = new("facet", ["2.8"]);

        const string expected = "le 2.8";

        // act
        var result = filterExpression.GetFilterExpression(context);

        // assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetFilterExpression_MultipleFacetValues_ThrowsArgumentException()
    {
        // arrange
        LessThanOrEqualToExpression filterExpression = new(new DefaultFilterExpressionFormatter());
        SearchFilterRequest context = new("facet", ["2.8", "3.4", "5.8"]);

        // act.
        Action failedGetFilterExpressionAction = () =>
            filterExpression.GetFilterExpression(context);

        //assert
        ArgumentException exception =
            Assert.Throws<ArgumentException>(failedGetFilterExpressionAction);
        Assert.Equal("Less than or equal to expression expects only one value. (Parameter 'facet')", exception.Message);
    }

    [Fact]
    public void GetFilterExpression_NonNumericFacetValue_ThrowsArgumentException()
    {
        // arrange
        LessThanOrEqualToExpression filterExpression = new(new DefaultFilterExpressionFormatter());
        SearchFilterRequest context = new("facet", ["HelloWorld"]);

        // act.
        Action failedGetFilterExpressionAction = () =>
            filterExpression.GetFilterExpression(context);

        //assert
        ArgumentException exception =
            Assert.Throws<ArgumentException>(failedGetFilterExpressionAction);
        Assert.Equal("Less than or equal to expression must be assigned a positive number or zero. (Parameter 'facet')", exception.Message);
    }

    [Fact]
    public void GetFilterExpression_NonNegativeFacetValue_ThrowsArgumentException()
    {
        // arrange
        LessThanOrEqualToExpression filterExpression = new(new DefaultFilterExpressionFormatter());
        SearchFilterRequest context = new("facet", ["-1.2"]);

        // act.
        Action failedGetFilterExpressionAction = () =>
            filterExpression.GetFilterExpression(context);

        //assert
        ArgumentException exception =
            Assert.Throws<ArgumentException>(failedGetFilterExpressionAction);
        Assert.Equal("Less than or equal to expression must be assigned a positive number or zero. (Parameter 'facet')", exception.Message);
    }

    [Fact]
    public void Ctor_NullFilterExpressionFormatter_ThrowsArgumentException()
    {
        // arrange/act.
        Action failedCtorAction = () =>
            new LessThanOrEqualToExpression(filterExpressionFormatter: null!);

        //assert
        ArgumentException exception =
            Assert.Throws<ArgumentNullException>(failedCtorAction);
        Assert.Equal("Value cannot be null. (Parameter 'filterExpressionFormatter')", exception.Message);
    }
}
