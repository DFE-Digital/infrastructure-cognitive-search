using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.FilterExpressions;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.FilterExpressions.Formatters;
using Xunit;

namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.Tests.Filtering.FilterExpressions;

public sealed class SearchGeoLocationFilterExpressionTests
{
    [Fact]
    public void GetFilterExpression_TwoFilterValues_ReturnsFormattedExpression()
    {
        // arrange
        SearchGeoLocationFilterExpression filterExpression = new(new DefaultFilterExpressionFormatter());
        SearchFilterRequest context = new("filter", ["2.23", "-1.34"]);

        const string expected = "geo.distance(Location,geography'POINT(2.23 -1.34)')";

        // act
        var result = filterExpression.GetFilterExpression(context);

        // assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Ctor_NullFilterExpressionFormatter_ThrowsArgumentException()
    {
        // arrange/act.
        Action failedCtorAction = () =>
            new SearchGeoLocationFilterExpression(filterExpressionFormatter: null!);

        //assert
        ArgumentException exception =
            Assert.Throws<ArgumentNullException>(failedCtorAction);
        Assert.Equal("Value cannot be null. (Parameter 'filterExpressionFormatter')", exception.Message);
    }

    [Fact]
    public void GetFilterExpression_MoreThanTwoFilterValues_ThrowsArgumentException()
    {
        // arrange
        SearchGeoLocationFilterExpression filterExpression = new(new DefaultFilterExpressionFormatter());
        SearchFilterRequest context = new("filter", ["2.23", "-1.34", "9.34"]);

        // act.
        Action failedGetFilterExpressionAction = () =>
            filterExpression.GetFilterExpression(context);

        //assert
        ArgumentException exception =
            Assert.Throws<ArgumentException>(failedGetFilterExpressionAction);
        Assert.Equal("The geo-location filter expression expects two values representing latitude and longitude. (Parameter 'filter')", exception.Message);
    }

    [Fact]
    public void GetFilterExpression_NonNumericFilterValues_ThrowsArgumentException()
    {
        // arrange
        SearchGeoLocationFilterExpression filterExpression = new(new DefaultFilterExpressionFormatter());
        SearchFilterRequest context = new("filter", ["Hello", "World"]);

        // act.
        Action failedGetFilterExpressionAction = () =>
            filterExpression.GetFilterExpression(context);

        //assert
        ArgumentException exception =
            Assert.Throws<ArgumentException>(failedGetFilterExpressionAction);
        Assert.Equal("Invalid geo-location point defined in arguments. (Parameter 'filter')", exception.Message);
    }

    [Fact]
    public void GetFilterExpression_MultipleFilterValuesAndDelimiterNotSpecifiedWhenNotRequired_ReturnsFormattedExpression()
    {
        // arrange
        SearchGeoLocationFilterExpression filterExpression = new(new DefaultFilterExpressionFormatter());
        SearchFilterRequest request = new("filter", ["1.0", "2.1"]);

        const string expected = "geo.distance(Location,geography'POINT(1.0 2.1)')";

        // act
        string result = filterExpression.GetFilterExpression(request);

        // assert
        Assert.Equal(expected, result);
    }
}