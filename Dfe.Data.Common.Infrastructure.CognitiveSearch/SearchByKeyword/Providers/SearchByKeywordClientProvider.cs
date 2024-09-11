using Azure;
using Azure.Search.Documents;
using Microsoft.Extensions.Options;

namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword.Providers;

/// <summary>
/// Provides a readily configured T:Azure.Search.Documents.SearchClient
/// for use when making search by key-word requests. The provider makes use of the
/// Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword.Providers.ISearchIndexNamesProvider
/// which aggregates the required search indexes and provisions the underlying SearchClient(s)
/// accessible by index name.
/// </summary>
public sealed class SearchByKeywordClientProvider : ISearchByKeywordClientProvider
{
    private readonly Options.AzureSearchConnectionOptions _azureSearchClientOptions;
    private readonly Dictionary<string, Lazy<SearchClient>> _lazySearchClients;

    /// <summary>
    /// The client provider uses a Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword.Providers.ISearchIndexNamesProvider
    /// to assign a T:Azure.Search.Documents.SearchClient instance to each index configured for search.
    /// </summary>
    /// <param name="azureSearchOptions">
    /// The native azure search options used to configure requests to the native azure service.
    /// </param>
    /// <param name="indexNamesProvider">
    /// The Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword.Providers.ISearchIndexNamesProvider
    /// instance used to determine the index(s) to configure a search client for.
    /// </param>
    public SearchByKeywordClientProvider(
        IOptions<Options.AzureSearchConnectionOptions> azureSearchOptions,
        ISearchIndexNamesProvider indexNamesProvider)
    {
        ArgumentNullException.ThrowIfNull(azureSearchOptions);

        _azureSearchClientOptions = azureSearchOptions.Value;
        _lazySearchClients = [];

        indexNamesProvider
            .GetIndexNames()
            .ToList()
            .ForEach(indexName =>
            {
                _lazySearchClients.Add(
                    indexName,
                     new Lazy<SearchClient>(() =>
                        CreateSearchClientInstance(indexName)));
            });
    }

    /// <summary>
    /// Invokes a search client instance based on the index name specified.
    /// </summary>
    /// <param name="indexName">
    /// The string name of the index under which to derive a configured search client instance.
    /// </param>
    /// <returns>
    /// A search client instance configured to search across the index (name) specified.
    /// </returns>
    /// <exception cref="SearchByKeywordClientInvocationException">
    /// Exception thrown when a search client cannot be derived for the index specified.
    /// </exception>
    public Task<SearchClient> InvokeSearchClientAsync(string indexName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(indexName);

        _lazySearchClients.
            TryGetValue(indexName, out Lazy<SearchClient>? _searchClient);

        if (_searchClient == null)
        {
            throw new SearchByKeywordClientInvocationException(indexName);
        }

        return Task.FromResult(_searchClient.Value);
    }

    /// <summary>
    /// Factory method for creating a search client instance for a given index (name)
    /// configured using the provisioned search endpoint Uri and credentials provided.
    /// </summary>
    /// <param name="indexName">
    /// The string name of the index under which to configure the search client.
    /// </param>
    /// <returns>
    /// A configured T:Azure.Search.Documents.SearchClient instance.
    /// </returns>
    private SearchClient CreateSearchClientInstance(string indexName)
    {
        ArgumentNullException.ThrowIfNull(indexName);
        ArgumentException.ThrowIfNullOrEmpty(_azureSearchClientOptions.EndpointUri);
        ArgumentException.ThrowIfNullOrEmpty(_azureSearchClientOptions.Credentials);

        return new SearchClient(
            endpoint: new Uri(
                _azureSearchClientOptions.EndpointUri),
            indexName: indexName,
            credential: new AzureKeyCredential(
                _azureSearchClientOptions.Credentials));
    }
}
