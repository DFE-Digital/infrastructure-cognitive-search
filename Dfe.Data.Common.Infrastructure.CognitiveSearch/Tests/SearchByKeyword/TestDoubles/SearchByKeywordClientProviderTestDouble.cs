using Azure;
using Azure.Search.Documents;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword.Providers;
using Moq;
using System.Dynamic;

namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.Tests.SearchByKeyword.TestDoubles;

internal static class SearchByKeywordClientProviderTestDouble
{
    public static Mock<ISearchByKeywordClientProvider> ISearchByKeywordClientProviderMock() => new();

    public static ISearchByKeywordClientProvider CreateWithDefaultResponse()
    {
        Mock<SearchClient> _azureSearchClientMock = new();

        var searchByKeywordClientProviderMock = ISearchByKeywordClientProviderMock();

        searchByKeywordClientProviderMock.Setup(_
            => _.InvokeSearchClientAsync(It.IsAny<string>())).ReturnsAsync(_azureSearchClientMock.Object);

        var searchResults =
            SearchResultsTestDouble<ExpandoObject>.SearchResultsWith(new ExpandoObject());

        _azureSearchClientMock.Setup(client =>
            client.SearchAsync<ExpandoObject>(
                It.IsAny<string>(), It.IsAny<SearchOptions>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Response.FromValue(searchResults, new Mock<Response>().Object));

        return searchByKeywordClientProviderMock.Object;
    }
}
