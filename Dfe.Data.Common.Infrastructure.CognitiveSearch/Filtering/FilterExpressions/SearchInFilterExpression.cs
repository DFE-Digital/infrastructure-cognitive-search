using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.FilterExpressions.Context;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.FilterExpressions.Formatters;

namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.FilterExpressions;

/// <summary>
/// 
/// </summary>
public sealed class SearchInFilterExpression : ISearchFilterExpression
{
    private readonly IFilterExpressionFormatter _filterExpressionFormatter;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="filterExpressionFormatter"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public SearchInFilterExpression(IFilterExpressionFormatter filterExpressionFormatter)
    {
        _filterExpressionFormatter =
            filterExpressionFormatter ??
            throw new ArgumentNullException(nameof(filterExpressionFormatter));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="searchFilterContext"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public string GetFilterExpression(SearchFilterContext searchFilterContext)
    {
        ArgumentNullException.ThrowIfNull(searchFilterContext);

        // search.in expressions can't be applied to booleans.
        searchFilterContext.FacetedValues.ToList()
            .ForEach(facetValue => {
                if (facetValue is bool){
                    throw new ArgumentException("Invalid boolean type argument for facet", searchFilterContext.Facet);
                }
            });

        _filterExpressionFormatter.SetExpressionParamsSeparator(",");

        return _filterExpressionFormatter
            .CreateFormattedExpression(
                $"search.in({searchFilterContext.Facet}, " +
                $"'{_filterExpressionFormatter.CreateFilterCriteriaPlaceholders(searchFilterContext.FacetedValues)}')",
                searchFilterContext.FacetedValues);
    }
}