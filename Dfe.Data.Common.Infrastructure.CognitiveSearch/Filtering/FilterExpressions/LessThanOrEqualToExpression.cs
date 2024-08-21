using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.FilterExpressions.Context;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.FilterExpressions.Formatters;

namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.FilterExpressions;

/// <summary>
/// 
/// </summary>
public sealed class LessThanOrEqualToExpression : ISearchFilterExpression
{
    private readonly IFilterExpressionFormatter _filterExpressionFormatter;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="filterExpressionFormatter"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public LessThanOrEqualToExpression(IFilterExpressionFormatter filterExpressionFormatter)
    {
        _filterExpressionFormatter =
            filterExpressionFormatter ??
            throw new ArgumentNullException(nameof(filterExpressionFormatter));
    }

    /// <summary>
    /// /
    /// </summary>
    /// <param name="searchFilterContext"></param>
    /// <returns></returns>
    public string CreateFilterExpression(SearchFilterContext searchFilterContext)
    {
        ArgumentNullException.ThrowIfNull(searchFilterContext);

        // TODO: add extra validation in here to ensure values sent can be parsed correctly.

        return
            _filterExpressionFormatter
                .CreateFormattedExpression(
                    $"le {_filterExpressionFormatter.CreateFilterCriteriaPlaceholders(searchFilterContext.FacetedValues)}",
                    searchFilterContext.FacetedValues);
    }
}
