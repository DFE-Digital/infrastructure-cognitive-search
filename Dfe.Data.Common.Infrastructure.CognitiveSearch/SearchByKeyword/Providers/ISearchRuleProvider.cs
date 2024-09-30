using Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword.Options;

namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword.Providers;

/// <summary>
/// Definition of a provider of search rule that can be used by the search implementation
/// </summary>
public interface ISearchRuleProvider
{
    /// <summary>
    /// Apply optional search rules set in the <see cref="SearchRuleOptions"/> to the search keyword
    /// </summary>
    /// <param name="keyword">
    /// the search keyword
    /// </param>
    /// <returns></returns>
    public string ApplySearchRules(string keyword);
}