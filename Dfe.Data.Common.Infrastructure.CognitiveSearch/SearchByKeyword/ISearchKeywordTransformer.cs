namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword;

/// <summary>
/// Definition of a contract of a transformer that can be used by the search implementation
/// </summary>
public interface ISearchKeywordTransformer
{
    /// <summary>
    /// Apply transformer to the search keyword
    /// </summary>
    /// <param name="keyword">
    /// the search keyword
    /// </param>
    /// <returns></returns>
    string Apply(string keyword);
}
