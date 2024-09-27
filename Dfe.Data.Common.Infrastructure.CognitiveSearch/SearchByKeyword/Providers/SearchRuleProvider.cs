using Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        if (_ruleOptions.SearchRule == "PartialWordMatch")
        {
            //var searchKeywords = searchKeyword.Replace(" ", "* ");
            searchKeyword = searchKeyword + "*";
        }
        return searchKeyword;
    }
}
