using Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword.IndexNames.Options;

namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword.IndexNames.Providers;
/// <summary>
/// 
/// </summary>
public sealed class AzureConfigurationSearchIndexNamesProvider : ISearchIndexNamesProvider
{
    private readonly IReadOnlyList<string> _indexNames;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="searchIndexNamesOptions"></param>
    /// <exception cref="ArgumentException"></exception>
    public AzureConfigurationSearchIndexNamesProvider(SearchIndexNamesOptions searchIndexNamesOptions)
    {
        ArgumentNullException.ThrowIfNull(searchIndexNamesOptions);

        if (searchIndexNamesOptions.Names is null || searchIndexNamesOptions.Names.Count == 0)
        {
            throw new ArgumentException("IndexNames cannot be null or empty");
        }

        _indexNames = searchIndexNamesOptions.Names;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public IEnumerable<string> GetIndexNames() => _indexNames;
}
