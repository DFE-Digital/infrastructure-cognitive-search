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

        _ = searchFilterExpressionFactoryMock
            .Setup(searchFilterExpressionFactory =>
                searchFilterExpressionFactory
                    .CreateFilter(It.IsAny<string>()))
                        .Returns((string type) =>
                        {
                            ISearchFilterExpression searchFilterExpression = null!;

                            switch (type)
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

                            return searchFilterExpression;
                        });

        return searchFilterExpressionFactoryMock.Object;
    }
}