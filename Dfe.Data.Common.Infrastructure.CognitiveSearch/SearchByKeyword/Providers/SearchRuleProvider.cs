using Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword.Options;

namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword.Providers;

/// <summary>
/// Facilitates search rules to be specified when running a search
/// </summary>
public class SearchRuleProvider : ISearchRuleProvider
{
    private SearchRuleOptions _ruleOptions;

    /// <summary>
    /// Construct the saerch rules provider, injecting into it the <see cref="SearchRuleOptions"/> to be applied
    /// </summary>
    /// <param name="searchRuleOptions"></param>
    public SearchRuleProvider(SearchRuleOptions searchRuleOptions)
    {
        _ruleOptions = searchRuleOptions;
    }

    /// <summary>
    /// Apply search rules as specified in the options
    /// </summary>
    /// <param name="searchKeyword"></param>
    /// <returns></returns>
    public string ApplySearchRules(string searchKeyword)
    {
        var searchKeywordWithRulesApplied = searchKeyword;
        if (_ruleOptions.SearchRule == "PartialWordMatch") // only 1 search rule so far
        {
            searchKeywordWithRulesApplied = searchKeywordWithRulesApplied.TrimEnd();
            searchKeywordWithRulesApplied = searchKeywordWithRulesApplied.Replace(" ", "* ");
            searchKeywordWithRulesApplied = searchKeywordWithRulesApplied + "*";
        }
        return searchKeywordWithRulesApplied;
    }
}
