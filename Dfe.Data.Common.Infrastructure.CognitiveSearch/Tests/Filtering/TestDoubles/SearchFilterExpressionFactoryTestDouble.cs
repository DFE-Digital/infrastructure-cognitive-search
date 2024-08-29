using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.FilterExpressions;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.FilterExpressions.Factories;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.FilterExpressions.Formatters;
using Moq;

namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.Tests.Filtering.FilterExpressions;

internal static class SearchFilterExpressionFactoryTestDouble
{
    public static Mock<ISearchFilterExpressionFactory> SearchFilterExpressionFactoryMock() => new();

    public static ISearchFilterExpressionFactory MockSearchFilterExpressionFactory() =>
        MockSearchFilterExpressionFactoryFor(new DefaultFilterExpressionFormatter());

    public static ISearchFilterExpressionFactory MockSearchFilterExpressionFactoryFor(IFilterExpressionFormatter filterExpressionFormatter)
    {
        var searchFilterExpressionFactoryMock = SearchFilterExpressionFactoryMock();

        searchFilterExpressionFactoryMock
            .Setup(searchFilterExpressionFactory =>
                searchFilterExpressionFactory
                    .CreateFilter(It.IsAny<string>()))
                        .Returns((string type) =>
                            GetSearchFilterExpression(type, filterExpressionFormatter));

        searchFilterExpressionFactoryMock
            .Setup(searchFilterExpressionFactory =>
                searchFilterExpressionFactory
                    .CreateFilter(It.IsAny<Type>()))
                        .Returns((Type type) =>
                            GetSearchFilterExpression(type.Name, filterExpressionFormatter));

        return searchFilterExpressionFactoryMock.Object;
    }

    public static ISearchFilterExpressionFactory MockSearchFilterExpressionFactoryFor<TFilterType>()
    {
        var searchFilterExpressionFactoryMock = SearchFilterExpressionFactoryMock();

        searchFilterExpressionFactoryMock
            .Setup(searchFilterExpressionFactory =>
                searchFilterExpressionFactory
                    .CreateFilter<ISearchFilterExpression>())
                        .Returns(() =>
                            GetSearchFilterExpression(
                                typeof(TFilterType).Name,
                                new DefaultFilterExpressionFormatter()));

        return searchFilterExpressionFactoryMock.Object;
    }

    private static ISearchFilterExpression GetSearchFilterExpression(
        string filterTypeName,
        IFilterExpressionFormatter filterExpressionFormatter)
    {
        ISearchFilterExpression searchFilterExpression = null!;

        switch (filterTypeName)
        {
            case "SearchInFilterExpression":
                searchFilterExpression =
                    new SearchInFilterExpression(filterExpressionFormatter);
                break;
            case "LessThanOrEqualToExpression":
                searchFilterExpression =
                    new LessThanOrEqualToExpression(filterExpressionFormatter);
                break;
            case "SearchGeoLocationFilterExpression":
                searchFilterExpression =
                    new SearchGeoLocationFilterExpression(filterExpressionFormatter);
                break;
        }
        ArgumentNullException.ThrowIfNullOrWhiteSpace(filterTypeName);

        return searchFilterExpression;
    }
}