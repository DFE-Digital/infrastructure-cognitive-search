namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword.IndexNames.Providers;

/// <summary>
/// Provides the contract by which to derive all the configured
/// indexes under which to search.
/// </summary>
public interface ISearchIndexNamesProvider
{
    /// <summary>
    /// Contract used to derive all index names configured for a given search.
    /// </summary>
    /// A collection of string index names.
    public IEnumerable<string> GetIndexNames();
}
