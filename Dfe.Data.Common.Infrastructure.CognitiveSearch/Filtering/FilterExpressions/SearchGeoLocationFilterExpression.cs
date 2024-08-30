using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.FilterExpressions.Formatters;

namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.FilterExpressions;

/// <summary>
/// Creates an OData filter expression which represents the <b>geo.distance</b> function supported by
/// Azure AI geo-spatial query capability. When using geo.distance the geo-point must conform to
/// the format "geography'POINT("lon lat")'" with only the longitude and latitude points expected
/// to be provisioned in a real number format. If the values are not provisioned correctly, i.e.
/// there are NOT exactly two longitude and latitude values passed in a real number format then
/// the appropriate exceptions will be thrown. For further information please visit the following link,
/// <a href="https://learn.microsoft.com/en-us/azure/search/search-query-odata-geo-spatial-functions">geo-spatial functions</a>.
/// For the complete OData expression syntax reference for Azure AI Search please visit the following link,
/// <a href="https://learn.microsoft.com/en-us/azure/search/search-query-odata-syntax-reference">OData syntax reference</a>.
/// </summary>
public sealed class SearchGeoLocationFilterExpression : ISearchFilterExpression
{
    private readonly IFilterExpressionFormatter _filterExpressionFormatter;

    /// <summary>
    /// The <see cref="SearchGeoLocationFilterExpression"/> uses a <see cref="IFilterExpressionFormatter"/>
    /// to help facilitate the creation of a fully configured <b>geo.distance</b> filter expression string.
    /// </summary>
    /// <param name="filterExpressionFormatter">
    /// Provides a convenient mechanism for creating composite format strings for use when generating Azure AI OData
    /// filter expressions in string format.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Exception thrown if a null <see cref="IFilterExpressionFormatter"/> is injected.
    /// </exception>
    public SearchGeoLocationFilterExpression(IFilterExpressionFormatter filterExpressionFormatter)
    {
        _filterExpressionFormatter =
            filterExpressionFormatter ??
            throw new ArgumentNullException(nameof(filterExpressionFormatter));
    }

    /// <summary>
    /// Creates the formatted <b>geo.distance</b> filter expression string based on the requirements specified in the
    /// <see cref="SearchFilterRequest"/> object. Validation is included to ensure the expression is configured
    /// correctly, i.e. filter request values are expected to represent longitude and latitude, so exactly
    /// two values are expected that must parse to real number values.
    /// </summary>
    /// <param name="searchFilterRequest">
    /// The <see cref="SearchFilterRequest"/> object carries the filter and values used to configure the filter expression.
    /// </param>
    /// <returns>
    /// A configured <b>geo.distance</b> OData Azure AI filter expression in string format.
    /// </returns>
    public string GetFilterExpression(SearchFilterRequest searchFilterRequest)
    {
        ArgumentNullException.ThrowIfNull(searchFilterRequest);

        // We expect only two parameters here, representing Latitude and longitude.
        if (searchFilterRequest.FilterValues.Length != 2){
            throw new ArgumentException(
                "The geo-location filter expression expects two values representing latitude and longitude.", searchFilterRequest.FilterKey);
        }

        // Ensure the geo-location points are in the correct format.
        searchFilterRequest.FilterValues.ToList()
            .ForEach(filterValue => {
                if (!float.TryParse(filterValue.ToString(), out _)){
                    throw new ArgumentException("Invalid geo-location point defined in arguments.", searchFilterRequest.FilterKey);
                }
            });

        _filterExpressionFormatter.SetExpressionParamsSeparator(" ");

        return _filterExpressionFormatter
            .CreateFormattedExpression(
                "geo.distance(Location,geography'POINT(" +
                $"{_filterExpressionFormatter.CreateFilterCriteriaPlaceholders(searchFilterRequest.FilterValues)})')",
                searchFilterRequest.FilterValues);
    }
}