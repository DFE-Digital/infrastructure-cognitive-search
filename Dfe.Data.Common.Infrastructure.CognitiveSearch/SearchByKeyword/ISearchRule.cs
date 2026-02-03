namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword;

/// <summary>
/// Definition of a contract of a search rule that can be used by the search implementation
/// </summary>
public interface ISearchRule
{
    /// <summary>
    /// Apply search rules to the search keyword
    /// </summary>
    /// <param name="keyword">
    /// the search keyword
    /// </param>
    /// <returns></returns>
    string ApplySearchRules(string keyword);
}
