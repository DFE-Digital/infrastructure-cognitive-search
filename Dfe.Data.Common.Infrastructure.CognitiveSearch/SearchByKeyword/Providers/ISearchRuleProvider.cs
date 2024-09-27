namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword.Providers;

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