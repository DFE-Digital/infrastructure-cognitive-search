using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.FilterExpressions;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.FilterExpressions.Factories;
using FluentAssertions;
using Xunit;

namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.Tests.Filtering.FilterExpressions.Factories;

public class SearchFilterExpressionFactoryTests
{
    [Fact]
    public void CreateFilter_WithValidFilterName_ReturnsExpectedConfiguredFilterFromfactory()
    {
        // arrange.
        ISearchFilterExpressionFactory searchFilterExpressionFactory =
            SearchFilterExpressionFactoryTestDouble.MockSearchFilterExpressionFactory();

        // act.
        ISearchFilterExpression searchFilterExpression =
            searchFilterExpressionFactory.CreateFilter("SearchInFilterExpression");

        // assert.
        searchFilterExpression.Should().NotBeNull();
        searchFilterExpression.Should().BeOfType<SearchInFilterExpression>();
    }

    [Fact]
    public void CreateFilter_WithValidFilterType_ReturnsExpectedConfiguredFilterFromfactory()
    {
        // arrange.
        ISearchFilterExpressionFactory searchFilterExpressionFactory =
            SearchFilterExpressionFactoryTestDouble.MockSearchFilterExpressionFactory();

        // act.
        ISearchFilterExpression searchFilterExpression =
            searchFilterExpressionFactory.CreateFilter(typeof(SearchInFilterExpression));

        // assert.
        searchFilterExpression.Should().NotBeNull();
        searchFilterExpression.Should().BeOfType<SearchInFilterExpression>();
    }

    [Fact]
    public void CreateFilter_WithValidFilterGenricTemplateType_ReturnsExpectedConfiguredFilterFromfactory()
    {
        // arrange.
        ISearchFilterExpressionFactory searchFilterExpressionFactory =
            SearchFilterExpressionFactoryTestDouble.MockSearchFilterExpressionFactory();

        // act.
        ISearchFilterExpression searchFilterExpression =
            searchFilterExpressionFactory.CreateFilter<SearchInFilterExpression>();

        // assert.
        searchFilterExpression.Should().NotBeNull();
        searchFilterExpression.Should().BeOfType<SearchInFilterExpression>();
    }

    [Fact]
    public void CreateFilter_WithNullFilterName_ThrowsExpectedException()
    {
        // arrange.
        ISearchFilterExpressionFactory searchFilterExpressionFactory =
            SearchFilterExpressionFactoryTestDouble.MockSearchFilterExpressionFactory();

        // act.
        Action failedCreateFilterAction = () =>
            searchFilterExpressionFactory.CreateFilter(filterName: null!);

        //assert
        ArgumentException exception =
            Assert.Throws<ArgumentNullException>(failedCreateFilterAction);
            Assert.Equal("Value cannot be null. (Parameter 'filterTypeName')", exception.Message);
    }

    [Fact]
    public void CreateFilter_WithEmptyFilterName_ThrowsExpectedException()
    {
        // arrange.
        ISearchFilterExpressionFactory searchFilterExpressionFactory =
            SearchFilterExpressionFactoryTestDouble.MockSearchFilterExpressionFactory();

        // act.
        Action failedCreateFilterAction = () =>
            searchFilterExpressionFactory.CreateFilter(filterName: "");

        //assert
        ArgumentException exception =
            Assert.Throws<ArgumentException>(failedCreateFilterAction);
            Assert.Equal("The value cannot be an empty string or composed entirely of whitespace. (Parameter 'filterTypeName')", exception.Message);
    }

    [Fact]
    public void CreateFilter_WithWhitespaceFilterName_ThrowsExpectedException()
    {
        // arrange.
        ISearchFilterExpressionFactory searchFilterExpressionFactory =
            SearchFilterExpressionFactoryTestDouble.MockSearchFilterExpressionFactory();

        // act.
        Action failedCreateFilterAction = () =>
            searchFilterExpressionFactory.CreateFilter(filterName: "    ");

        //assert
        ArgumentException exception =
            Assert.Throws<ArgumentException>(failedCreateFilterAction);
            Assert.Equal("The value cannot be an empty string or composed entirely of whitespace. (Parameter 'filterTypeName')", exception.Message);
    }

    [Fact]
    public void CreateFilter_WithUnknownFilterName_ThrowsExpectedException()
    {
        // arrange.
        ISearchFilterExpressionFactory searchFilterExpressionFactory =
            SearchFilterExpressionFactoryTestDouble.MockSearchFilterExpressionFactory();

        // act.
        Action failedCreateFilterAction = () =>
            searchFilterExpressionFactory.CreateFilter(filterName: "unknownFilter");

        //assert
        ArgumentException exception =
            Assert.Throws<ArgumentOutOfRangeException>(failedCreateFilterAction);
            Assert.Equal("Specified argument was out of the range of valid values. (Parameter 'Search expression filter of type unknownFilter is not registered.')", exception.Message);
    }
}