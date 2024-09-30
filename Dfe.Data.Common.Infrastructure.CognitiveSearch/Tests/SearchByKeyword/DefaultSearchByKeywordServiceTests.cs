using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword.Options;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword.Providers;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.Tests.Filtering.TestDoubles;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.Tests.Search.TestDoubles;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.Tests.SearchByKeyword.TestDoubles;
using Moq;
using System.Data;
using Xunit;

namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.Tests.SearchByKeyword;

public class DefaultSearchByKeywordServiceTests
{
    private readonly Mock<ISearchByKeywordClientProvider> _searchClientProviderMock = new();
    private readonly Mock<SearchClient> _azureSearchClientMock = new();

    [Fact]
    public async Task SearchAsync_NoProvider_UsesUnmodifiedSearchTerm()
    {
        // arrange
        const string searchIndex = "index1";
        const string searchKeyword = "name";
        const string documentContentValue = "example name";

        SearchOptions searchOptions = AzureSearchOptionsTestDouble.SearchOptionsWithSearchField(searchKeyword);
        var searchResults = SearchResultsTestDouble<TestDocument>.SearchResultsWith(new TestDocument() { Name = documentContentValue });
        _searchClientProviderMock.Setup(provider => provider.InvokeSearchClientAsync(searchIndex))
            .ReturnsAsync(_azureSearchClientMock.Object);
        _azureSearchClientMock.Setup(client => client.SearchAsync<TestDocument>(searchKeyword, searchOptions, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Response.FromValue(searchResults, new Mock<Response>().Object));

        var searchService = new DefaultSearchByKeywordService(_searchClientProviderMock.Object);

        // act
        var result = (await searchService.SearchAsync<TestDocument>(searchKeyword, searchIndex, searchOptions)).Value.GetResults();

        // assert
        _azureSearchClientMock.Verify(search => search.SearchAsync<TestDocument>(searchKeyword, It.IsAny<SearchOptions>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task SearchAsync_CallsSearchRuleProvider()
    {
        // arrange
        const string searchIndex = "index1";
        const string searchKeyword = "name";
        const string searchKeywordOut = "name*";
        const string documentContentValue = "example name";
        var mockSearchRuleProvider = SearchRuleProviderTestDouble.MockFor(searchKeyword, searchKeywordOut);

        SearchOptions searchOptions = AzureSearchOptionsTestDouble.SearchOptionsWithSearchField(It.IsAny<string>());
        var searchResults = SearchResultsTestDouble<TestDocument>.SearchResultsWith(new TestDocument() { Name = documentContentValue });
        _searchClientProviderMock.Setup(provider => provider.InvokeSearchClientAsync(It.IsAny<string>()))
            .ReturnsAsync(_azureSearchClientMock.Object);
        _azureSearchClientMock.Setup(client => client.SearchAsync<TestDocument>(It.IsAny<string>(), searchOptions, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Response.FromValue(searchResults, new Mock<Response>().Object));

        var searchService = new DefaultSearchByKeywordService(_searchClientProviderMock.Object, mockSearchRuleProvider);

        // act
        var result = (await searchService.SearchAsync<TestDocument>(searchKeyword, searchIndex, searchOptions)).Value.GetResults();

        // assert
        Mock.Get(mockSearchRuleProvider).Verify(searchRuleProvider => searchRuleProvider.ApplySearchRules(searchKeyword), Times.Once);
        _azureSearchClientMock.Verify(search => search.SearchAsync<TestDocument>(searchKeywordOut, It.IsAny<SearchOptions>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task SearchAsync_ReturnsExpected()
    {
        // arrange
        const string searchIndex = "index1";
        const string searchKeyword = "name";
        const string documentContentValue = "example name";

        SearchOptions searchOptions = AzureSearchOptionsTestDouble.SearchOptionsWithSearchField(searchKeyword);
        var searchResults = SearchResultsTestDouble<TestDocument>.SearchResultsWith(new TestDocument() { Name = documentContentValue });
        _azureSearchClientMock
            .Setup(client => client.SearchAsync<TestDocument>(searchKeyword, searchOptions, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Response.FromValue(searchResults, new Mock<Response>().Object));
        _searchClientProviderMock.Setup(provider => provider.InvokeSearchClientAsync(It.IsAny<string>()))
            .ReturnsAsync(_azureSearchClientMock.Object);
        var searchService = new DefaultSearchByKeywordService(_searchClientProviderMock.Object);

        // act
        var result = (await searchService.SearchAsync<TestDocument>(searchKeyword, searchIndex, searchOptions)).Value.GetResults();

        // assert
        var firstPageResult = Assert.IsType<SearchResult<TestDocument>>(result.First());
        Assert.Equal(documentContentValue, firstPageResult.Document.Name);
    }

    [Theory]
    [InlineData("searchKeywordValue", null)]
    [InlineData(null, "searchIndexValue")]
    public async Task SearchAsync_NullArgument_ThrowsArgumentNullException(string searchKeyword, string searchIndex)
    {
        // arrange
        const string documentContentValue = "example name";

        SearchOptions searchOptions = AzureSearchOptionsTestDouble.SearchOptionsBasic();
        var searchResults =
            SearchResultsTestDouble<TestDocument>.SearchResultsWith(
                new TestDocument() { Name = documentContentValue });

        _searchClientProviderMock
            .Setup(provider => provider.InvokeSearchClientAsync(searchIndex))
            .ReturnsAsync(_azureSearchClientMock.Object);
        _azureSearchClientMock
            .Setup(client => client.SearchAsync<TestDocument>(searchKeyword, searchOptions, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Response.FromValue(searchResults, new Mock<Response>().Object));

        var searchService = new DefaultSearchByKeywordService(_searchClientProviderMock.Object);

        // act, assert
        _ = await Assert.ThrowsAsync<ArgumentNullException>(() =>
            searchService.SearchAsync<TestDocument>(searchKeyword, searchIndex, searchOptions));
    }

    [Theory]
    [InlineData("searchKeywordValue", "")]
    [InlineData("", "searchIndexValue")]
    public async Task SearchAsync_EmptyArgument_ThrowsArgumentException(string searchKeyword, string searchIndex)
    {
        // arrange
        const string documentContentValue = "example name";

        SearchOptions searchOptions = AzureSearchOptionsTestDouble.SearchOptionsBasic();
        var searchResults =
            SearchResultsTestDouble<TestDocument>.SearchResultsWith(
                new TestDocument() { Name = documentContentValue });

        _searchClientProviderMock
            .Setup(provider => provider.InvokeSearchClientAsync(searchIndex))
            .ReturnsAsync(_azureSearchClientMock.Object);
        _azureSearchClientMock
            .Setup(client => client.SearchAsync<TestDocument>(searchKeyword, searchOptions, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Response.FromValue(searchResults, new Mock<Response>().Object));

        var searchService = new DefaultSearchByKeywordService(_searchClientProviderMock.Object);

        // act, assert
        _ = await Assert.ThrowsAsync<ArgumentException>(() =>
            searchService.SearchAsync<TestDocument>(searchKeyword, searchIndex, searchOptions));
    }

    [Fact]
    public async Task SearchAsync_MissingSearchOptions_ThrowsArgumentNullException()
    {
        // arrange
        const string searchIndex = "index1";
        const string searchKeyword = "name";
        const string documentContentValue = "example name";

        var searchResults =
            SearchResultsTestDouble<TestDocument>.SearchResultsWith(
                new TestDocument() { Name = documentContentValue });

        _searchClientProviderMock
            .Setup(provider => provider.InvokeSearchClientAsync(searchIndex))
            .ReturnsAsync(_azureSearchClientMock.Object);
        _azureSearchClientMock
            .Setup(client => client.SearchAsync<TestDocument>(searchKeyword, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Response.FromValue(searchResults, new Mock<Response>().Object));

        var searchService = new DefaultSearchByKeywordService(_searchClientProviderMock.Object);

        // act, assert
        _ = await Assert.ThrowsAsync<ArgumentNullException>(() =>
            searchService.SearchAsync<TestDocument>(searchKeyword, searchIndex, null!));
    }

    private class TestDocument
    {
        public string? Name { get; set; }
    }
}
