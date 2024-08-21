using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.FilterExpressions;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.FilterExpressions.Context;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.FilterExpressions.Factories;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.LogicalOperators;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.LogicalOperators.Factories;
using System.Text;

namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering;

/// <summary>
/// 
/// </summary>
public sealed class SearchFilterExpressionBuilder : ISearchFilterExpressionBuilder
{
    private readonly ISearchFilterExpressionFactory _searchFilterExpressionFactory;
    private readonly ILogicalOperatorFactory _logicalOperatorFactory;
    private readonly StringBuilder _aggregatedSearchFilterExpression = new();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="searchFilterExpressionFactory"></param>
    /// <param name="logicalOperatorFactory"></param>
    public SearchFilterExpressionBuilder(
        ISearchFilterExpressionFactory searchFilterExpressionFactory,
        ILogicalOperatorFactory logicalOperatorFactory)
    {
        _searchFilterExpressionFactory = searchFilterExpressionFactory;
        _logicalOperatorFactory = logicalOperatorFactory;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="searchFilterContexts"></param>
    /// <returns></returns>
    public string BuildSearchFilter(IEnumerable<SearchFilterContext> searchFilterContexts)
    {
        // TODO: we only want to create these if they're in the map!!!!
        List<string> searchFilters = [];

        foreach (SearchFilterContext searchFilterContext in searchFilterContexts)
        {
            // TODO: we need to get the expression key from a map and maybe the logic operator.
            ISearchFilterExpression searchFilterExpression = _searchFilterExpressionFactory.CreateFilter("SearchInFilterExpression");
            searchFilters.Add(searchFilterExpression.CreateFilterExpression(searchFilterContext));
        }

        // TODO: we need to get the logical operator from the map (i.e. default bing operator)
        ILogicalOperator logicalOperator = _logicalOperatorFactory.CreateLogicalOperator("AndLogicalOperator");

        _aggregatedSearchFilterExpression.AppendJoin(logicalOperator.CreateLogicalOperator(), searchFilters);

        return _aggregatedSearchFilterExpression.ToString();
    }
}