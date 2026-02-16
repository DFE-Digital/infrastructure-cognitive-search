using Azure;
using Azure.Search.Documents.Indexes;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword.Options;
using Microsoft.Extensions.Options;

namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword.IndexNames.Providers;

/// <summary>
/// Provides the means by which to derive all the configured
/// indexes under which to search, established under configuration.
/// accessible by index name.
/// </summary>
public sealed class AzureRemoteSearchIndexNamesProvider : ISearchIndexNamesProvider
{
    private readonly AzureSearchConnectionOptions _azureSearchOptions;

    /// <summary>
    /// The index names provider uses a Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword.Options.SearchByKeywordClientOptions
    /// to establish the Uri endpoint and credentials used to invoke a the T:Azure.Search.Documents.Indexes.SearchIndexClient instance,
    /// that is used to manage indexes on a Search service.
    /// </summary>
    /// <param name="azureSearchOptions">
    /// Configuration options used to define the internal Azure
    /// cognitive search service credentials and Uri endpoint.
    /// </param>
    public AzureRemoteSearchIndexNamesProvider(IOptions<AzureSearchConnectionOptions> azureSearchOptions)
    {
        ArgumentNullException.ThrowIfNull(azureSearchOptions);

        _azureSearchOptions = azureSearchOptions.Value;
    }

    /// <summary>
    /// Gets the index names associated with a given search client search.
    /// </summary>
    /// <returns>
    /// A collection of string index names.
    /// </returns>
    public IEnumerable<string> GetIndexNames()
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(_azureSearchOptions.EndpointUri);
        ArgumentException.ThrowIfNullOrWhiteSpace(_azureSearchOptions.Credentials);

        List<string> indexNames = [];

        SearchIndexClient client =
            new(
                endpoint: new Uri(_azureSearchOptions.EndpointUri),
                credential: new AzureKeyCredential(_azureSearchOptions.Credentials));

        client.GetIndexNames().ToList()
            .ForEach(index => indexNames.Add(index));

        return indexNames;
    }
}
