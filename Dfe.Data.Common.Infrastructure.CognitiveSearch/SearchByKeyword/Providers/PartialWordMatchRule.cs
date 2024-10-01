using Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword.Options;
using System.Text;

namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword.Providers;

/// <summary>
/// Facilitates search rules to be specified when running a search
/// </summary>
public class PartialWordMatchRule : ISearchRule
{
    private SearchRuleOptions _ruleOptions;

    /// <summary>
    /// Construct the saerch rules provider, injecting into it the <see cref="SearchRuleOptions"/> to be applied
    /// </summary>
    /// <param name="searchRuleOptions"></param>
    public PartialWordMatchRule(SearchRuleOptions searchRuleOptions)
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
        if (_ruleOptions.SearchRule == "PartialWordMatch") // only 1 search rule so far
        {
            return new StringBuilder(searchKeyword.TrimEnd())
                .Replace(" ", "* ")
                .Append('*')
                .ToString();
        }
        return searchKeyword;
    }
}
