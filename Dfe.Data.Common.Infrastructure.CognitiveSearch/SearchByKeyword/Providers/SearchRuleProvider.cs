using Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword.Options;

namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword.Providers;

public class SearchRuleProvider : ISearchRuleProvider
{
    private SearchRuleOptions _ruleOptions;

    public SearchRuleProvider(SearchRuleOptions searchRuleOptions)
    {
        _ruleOptions = searchRuleOptions;
    }

    public string ApplySearchRules(string searchKeyword)
    {
        var searchKeywordWithRulesApplied = searchKeyword;
        if (_ruleOptions.SearchRule == "PartialWordMatch")
        {
            searchKeywordWithRulesApplied = searchKeywordWithRulesApplied.TrimEnd();
            searchKeywordWithRulesApplied = searchKeywordWithRulesApplied.Replace(" ", "* ");
            searchKeywordWithRulesApplied = searchKeywordWithRulesApplied + "*";
        }
        return searchKeywordWithRulesApplied;
    }
}
