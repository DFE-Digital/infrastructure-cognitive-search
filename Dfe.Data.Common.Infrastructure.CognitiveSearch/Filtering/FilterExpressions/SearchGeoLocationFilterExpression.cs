using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.FilterExpressions.Formatters;
using static System.Net.WebRequestMethods;

namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.FilterExpressions;

/// <summary>
/// Creates an OData filter expression which represents the geo.distance function supported by
/// Azure AI geo-spatial query capability. When using geo.distance the geo-point must conform to
/// the format "geography'POINT("lon lat")'" with only the longitude and latitude points expected
/// to be provisioned in a real number format. If the values are not provisioned correctly, i.e.
/// there are NOT exactly two longitude and latitude values passed in a real number format then
/// the appropriate exceptions will be thrown. For further information please visit the following link,
/// <see href="https://learn.microsoft.com/en-us/azure/search/search-query-odata-geo-spatial-functions"/>.
/// For the complete OData expression syntax reference for Azure AI Search please visit the following link,
/// <see href="https://learn.microsoft.com/en-us/azure/search/search-query-odata-syntax-reference"/>.
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
    public string GetFilterExpression(SearchFilterRequest searchFilterContext)
    {
        ArgumentNullException.ThrowIfNull(searchFilterContext);

        // We expect only two parameters here, representing Latitude and longitude.
        if (searchFilterContext.FilterValues.Length != 2){
            throw new ArgumentException(
                "The geo-location filter expression expects two values representing latitude and longitude.", searchFilterContext.FilterKey);
        }

        // Ensure the geo-location points are in the correct format.
        searchFilterContext.FilterValues.ToList()
            .ForEach(filterValue => {
                if (!float.TryParse(filterValue.ToString(), out _)){
                    throw new ArgumentException("Invalid geo-location point defined in arguments.", searchFilterContext.FilterKey);
                }
            });

        _filterExpressionFormatter.SetExpressionParamsSeparator(" ");

        return _filterExpressionFormatter
            .CreateFormattedExpression(
                "geo.distance(Location,geography'POINT(" +
                $"{_filterExpressionFormatter.CreateFilterCriteriaPlaceholders(searchFilterContext.FilterValues)})')",
                searchFilterContext.FilterValues);
    }
}