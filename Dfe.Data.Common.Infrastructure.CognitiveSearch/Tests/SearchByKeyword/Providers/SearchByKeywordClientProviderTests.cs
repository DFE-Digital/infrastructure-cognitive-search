using Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword.IndexNames.Providers;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword.Providers;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.Tests.Search.TestDoubles;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.Tests.SearchByKeyword.TestDoubles.StubBuilders;
using FluentAssertions;
using Moq;
using Xunit;

namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.Tests.SearchByKeyword.Providers;

public class SearchByKeywordClientProviderTests
{
    private readonly Mock<ISearchIndexNamesProvider> _searchIndexNamesProvider = new();

    [Fact]
    public async Task InvokeSearchClientAsync_WithValidParameters_ReturnsSearchClient()
    {
        // arrange
        var indexNameList = new List<string>() { "index1", "index2" };
        var searchOptions = new SearchByKeywordClientOptionsBuilder().Create();
        var options = IOptionsTestDouble.IOptionsMockFor(searchOptions);

        _searchIndexNamesProvider.Setup(provider => provider.GetIndexNames())
            .Returns(indexNameList);

        var searchClientProvider = new SearchByKeywordClientProvider(options, _searchIndexNamesProvider.Object);

        // act
        var result1 = await searchClientProvider.InvokeSearchClientAsync(indexNameList[0]);
        var result2 = await searchClientProvider.InvokeSearchClientAsync(indexNameList[1]);

        // assert
        Assert.NotNull(result1);
        Assert.Equal(searchOptions.EndpointUri, result1.Endpoint.AbsoluteUri);
        Assert.Equal(indexNameList[0], result1.IndexName);
        Assert.NotNull(result2);
        Assert.Equal(searchOptions.EndpointUri, result2.Endpoint.AbsoluteUri);
        Assert.Equal(indexNameList[1], result2.IndexName);
    }

    [Fact]
    public void InvokeSearchClientAsync_WithInvalidSearchOptionsUri_ThrowsUriFormatException()
    {
        // arrange
        const string indexName = "index1";

        var searchOptions = new SearchByKeywordClientOptionsBuilder().WithInvalidEndpoint().Create();
        var options = IOptionsTestDouble.IOptionsMockFor(searchOptions);
        _searchIndexNamesProvider.Setup(provider =>
            provider.GetIndexNames()).Returns([indexName]);

        var searchClientProvider = new SearchByKeywordClientProvider(options, _searchIndexNamesProvider.Object);

        // act, assert
        _ = searchClientProvider
            .Invoking(async provider =>
                await provider.InvokeSearchClientAsync(indexName))
                    .Should()
                        .ThrowAsync<ArgumentException>()
                        .WithMessage("The value cannot be an empty string. (Parameter '_azureSearchClientOptions.EndpointUri')");
    }

    [Fact]
    public void InvokeSearchClientAsync_WithInvalidSearchOptionsCredentials_ThrowsArgumentException()
    {
        // arrange
        const string indexName = "index1";

        var searchOptions = new SearchByKeywordClientOptionsBuilder().WithInvalidCredentials().Create();
        var options = IOptionsTestDouble.IOptionsMockFor(searchOptions);
        _searchIndexNamesProvider.Setup(provider =>
            provider.GetIndexNames()).Returns([indexName]);

        var searchClientProvider = new SearchByKeywordClientProvider(options, _searchIndexNamesProvider.Object);

        // act, assert
        _ = searchClientProvider
           .Invoking(async provider =>
               await provider.InvokeSearchClientAsync(indexName))
                   .Should()
                       .ThrowAsync<ArgumentException>()
                       .WithMessage("The value cannot be an empty string. (Parameter '_azureSearchClientOptions.Credentials')");
    }

    [Fact]
    public void InvokeSearchClientAsync_WithEmptySearchOptionsIndex_ThrowsSearchClientInvocationException()
    {
        // arrange
        const string indexName = "index1";

        var searchOptions = new SearchByKeywordClientOptionsBuilder().Create();
        var options = IOptionsTestDouble.IOptionsMockFor(searchOptions);
        _searchIndexNamesProvider.Setup(provider =>
            provider.GetIndexNames()).Returns([]);

        var searchClientProvider = new SearchByKeywordClientProvider(options, _searchIndexNamesProvider.Object);

        // act, assert
        _ = searchClientProvider
          .Invoking(async provider =>
              await provider.InvokeSearchClientAsync(indexName))
                  .Should()
                      .ThrowAsync<ArgumentException>()
                      .WithMessage("The value cannot be an empty string. (Parameter 'indexName')");
    }

    [Fact]
    public async Task InvokeSearchClientAsync_WithMismatchedIndex_ThrowsSearchClientInvocationException()
    {
        // arrange
        const string searchOptionsIndexName = "index1";
        const string invokeIndexName = "differentIndex";

        var searchOptions = new SearchByKeywordClientOptionsBuilder().Create();
        var options = IOptionsTestDouble.IOptionsMockFor(searchOptions);
        _searchIndexNamesProvider.Setup(provider =>
            provider.GetIndexNames()).Returns([searchOptionsIndexName]);

        var searchClientProvider = new SearchByKeywordClientProvider(options, _searchIndexNamesProvider.Object);

        // act, assert
        _ = await Assert.ThrowsAsync<SearchByKeywordClientInvocationException>(()
            => searchClientProvider.InvokeSearchClientAsync(invokeIndexName));
    }
}
