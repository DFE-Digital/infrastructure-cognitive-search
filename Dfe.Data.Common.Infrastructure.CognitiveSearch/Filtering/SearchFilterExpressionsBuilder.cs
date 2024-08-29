using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.FilterExpressions;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.FilterExpressions.Factories;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.LogicalOperators;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.LogicalOperators.Factories;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.Options;
using Microsoft.Extensions.Options;
using System.Collections.ObjectModel;
using System.Text;

namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering;

/// <summary>
/// 
/// </summary>
public sealed class SearchFilterExpressionsBuilder : ISearchFilterExpressionsBuilder
{
    private readonly ISearchFilterExpressionFactory _searchFilterExpressionFactory;
    private readonly ILogicalOperatorFactory _logicalOperatorFactory;
    private readonly StringBuilder _aggregatedSearchFilterExpression = new();
    private readonly FilterKeyToFilterExpressionMapOptions _filterKeyToFilterExpressionMapOptions;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="searchFilterExpressionFactory"></param>
    /// <param name="logicalOperatorFactory"></param>
    /// <param name="filterKeyToFilterExpressionMapOptions"></param>
    public SearchFilterExpressionsBuilder(
        ISearchFilterExpressionFactory searchFilterExpressionFactory,
        ILogicalOperatorFactory logicalOperatorFactory,
        IOptions<FilterKeyToFilterExpressionMapOptions> filterKeyToFilterExpressionMapOptions)
    {
        _searchFilterExpressionFactory = searchFilterExpressionFactory;
        _logicalOperatorFactory = logicalOperatorFactory;
        ArgumentNullException.ThrowIfNull(filterKeyToFilterExpressionMapOptions);
        _filterKeyToFilterExpressionMapOptions = filterKeyToFilterExpressionMapOptions.Value;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="searchFilterContexts"></param>
    /// <returns></returns>
    public string BuildSearchFilterExpressions(IEnumerable<SearchFilterRequest> searchFilterContexts)
    {
        IEnumerable<string> searchFilters = GetValidSearchFilterExpression(searchFilterContexts);
        ILogicalOperator logicalOperator = GetDefaultLogicalOperator();

        _aggregatedSearchFilterExpression.AppendJoin(logicalOperator.GetOperatorExpression(), searchFilters);

        return _aggregatedSearchFilterExpression.ToString();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="searchFilterContexts"></param>
    /// <returns></returns>
    private ReadOnlyCollection<string> GetValidSearchFilterExpression(IEnumerable<SearchFilterRequest> searchFilterContexts)
    {
        List<string> searchFilters = [];

        // Only derive search expressions that are recognised through the filter key to expression map.
        foreach (SearchFilterRequest searchFilterRequest in searchFilterContexts
            .Where(searchFilterContext =>
                _filterKeyToFilterExpressionMapOptions
                    .SearchFilterToExpressionMap.ContainsKey(searchFilterContext.FilterKey)))
        {
            ISearchFilterExpression searchFilterExpression =
                _searchFilterExpressionFactory.CreateFilter(
                    _filterKeyToFilterExpressionMapOptions.SearchFilterToExpressionMap[searchFilterRequest.FilterKey]);

            searchFilters.Add(searchFilterExpression.GetFilterExpression(searchFilterRequest));
        }

        return searchFilters.AsReadOnly();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    private ILogicalOperator GetDefaultLogicalOperator()
    {
        string defaultLogicalOperatorKey =
            !string.IsNullOrWhiteSpace(_filterKeyToFilterExpressionMapOptions.DefaultLogicalOperator) ?
            _filterKeyToFilterExpressionMapOptions.DefaultLogicalOperator :
                throw new ArgumentException("Unable to assign a null or empty logical operator to the search expression chain.");

        return _logicalOperatorFactory.CreateLogicalOperator(defaultLogicalOperatorKey);
    }
}