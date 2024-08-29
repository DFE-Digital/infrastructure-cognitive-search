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

        // We expect only two parameters here, representing Latitude and longitude.
        if (searchFilterContext.FacetedValues.Length != 2){
            throw new ArgumentException(
                "The geo-location filter expression expects two values representing latitude and longitude.", searchFilterContext.Facet);
        }

        // Ensure the geo-location points are in the correct format.
        searchFilterContext.FacetedValues.ToList()
            .ForEach(facetValue => {
                if (!float.TryParse(facetValue.ToString(), out _)){
                    throw new ArgumentException("Invalid geo-location point defined in arguments.", searchFilterContext.Facet);
                }
            });

        _filterExpressionFormatter.SetExpressionParamsSeparator(" ");

        return
            _filterExpressionFormatter
                .CreateFormattedExpression(
                    "geo.distance(Location,geography'POINT(" +
                    $"{_filterExpressionFormatter.CreateFilterCriteriaPlaceholders(searchFilterContext.FacetedValues)})')",
                    searchFilterContext.FacetedValues);
    }
}