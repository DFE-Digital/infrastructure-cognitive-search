using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.FilterExpressions.Context;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.FilterExpressions.Formatters;

namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.FilterExpressions;

/// <summary>
/// 
/// </summary>
public sealed class SearchGeoLocationFilterExpression : ISearchFilterExpression
{
    private readonly IFilterExpressionFormatter _filterExpressionFormatter;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="filterExpressionFormatter"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public SearchGeoLocationFilterExpression(IFilterExpressionFormatter filterExpressionFormatter)
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
    public string GetFilterExpression(SearchFilterContext searchFilterContext)
    {
        ArgumentNullException.ThrowIfNull(searchFilterContext);

        // TODO: add extra validation in here to ensure values sent can be parsed correctly, i.e. geo-points are withing the real number range!

        _filterExpressionFormatter.SetExpressionParamsSeparator(" ");

        return
            _filterExpressionFormatter
                .CreateFormattedExpression(
                    "geo.distance(Location,geography'POINT(" +
                    $"{_filterExpressionFormatter.CreateFilterCriteriaPlaceholders(searchFilterContext.FacetedValues)})')",
                    searchFilterContext.FacetedValues);
    }
}