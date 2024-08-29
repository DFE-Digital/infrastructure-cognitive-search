using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.FilterExpressions;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.FilterExpressions.Context;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.FilterExpressions.Formatters;
using Xunit;

namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.Tests.Filtering.FilterExpressions;

public sealed class SearchGeoLocationFilterExpressionTests
{
    [Fact]
    public void GetFilterExpression_TwoFacetValues_ReturnsFormattedExpression()
    {
        // arrange
        SearchGeoLocationFilterExpression filterExpression = new(new DefaultFilterExpressionFormatter());
        SearchFilterContext context = new("facet", ["2.23", "-1.34"]);

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
    public void GetFilterExpression_MoreThanTwoFacetValues_ThrowsArgumentException()
    {
        // arrange
        SearchGeoLocationFilterExpression filterExpression = new(new DefaultFilterExpressionFormatter());
        SearchFilterContext context = new("facet", ["2.23", "-1.34", "9.34"]);

        // act.
        Action failedGetFilterExpressionAction = () =>
            filterExpression.GetFilterExpression(context);

        //assert
        ArgumentException exception =
            Assert.Throws<ArgumentException>(failedGetFilterExpressionAction);
        Assert.Equal("The geo-location filter expression expects two values representing latitude and longitude. (Parameter 'facet')", exception.Message);
    }

    [Fact]
    public void GetFilterExpression_NonNumericFacetValues_ThrowsArgumentException()
    {
        // arrange
        SearchGeoLocationFilterExpression filterExpression = new(new DefaultFilterExpressionFormatter());
        SearchFilterContext context = new("facet", ["Hello", "World"]);

        // act.
        Action failedGetFilterExpressionAction = () =>
            filterExpression.GetFilterExpression(context);

        //assert
        ArgumentException exception =
            Assert.Throws<ArgumentException>(failedGetFilterExpressionAction);
        Assert.Equal("Invalid geo-location point defined in arguments. (Parameter 'facet')", exception.Message);
    }
}