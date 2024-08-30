using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.FilterExpressions.Formatters;

namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.FilterExpressions;

/// <summary>
/// Creates an OData filter expression which represents the <b>search.in</b> function supported by
/// Azure AI search filter capability. When using search.in the values/variables provisioned
/// are generally prescribed to string formats (it cannot handle boolean types for example).
/// If the values are not provisioned correctly, i.e. a boolean is provisioned then the
/// appropriate exception will be thrown. For further information please visit the following link,
/// <a href="https://learn.microsoft.com/en-us/azure/search/search-query-odata-search-in-function">
/// OData search in function</a>.
/// </summary>
public sealed class SearchInFilterExpression : ISearchFilterExpression
{
    private readonly IFilterExpressionFormatter _filterExpressionFormatter;

    /// <summary>
    /// The <see cref="SearchInFilterExpression"/> uses a <see cref="IFilterExpressionFormatter"/>
    /// to help facilitate the creation of a fully configured <b>search.in</b> filter expression string.
    /// </summary>
    /// <param name="filterExpressionFormatter">
    /// Provides a convenient mechanism for creating composite format strings for use when generating Azure AI OData
    /// filter expressions in string format.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Exception thrown if a null <see cref="IFilterExpressionFormatter"/> is injected.
    /// </exception>
    public SearchInFilterExpression(IFilterExpressionFormatter filterExpressionFormatter)
    {
        _filterExpressionFormatter =
            filterExpressionFormatter ??
            throw new ArgumentNullException(nameof(filterExpressionFormatter));
    }

    /// <summary>
    /// Creates the formatted <b>search.in</b> filter expression string based on the requirements specified in the
    /// <see cref="SearchFilterRequest"/> object. Validation is included to ensure the expression is configured
    /// correctly, i.e. filter request values are expected to parsable to strings.
    /// </summary>
    /// <param name="searchFilterRequest">
    /// The <see cref="SearchFilterRequest"/> object carries the filter and values used to configure the filter expression.
    /// </param>
    /// <returns>
    /// A configured <b>search.in</b> OData Azure AI filter expression in string format.
    /// </returns>
    public string GetFilterExpression(SearchFilterRequest searchFilterRequest)
    {
        ArgumentNullException.ThrowIfNull(searchFilterRequest);

        // search.in expressions can't be applied to booleans.
        searchFilterRequest.FilterValues.ToList()
            .ForEach(filterValue => {
                if (filterValue is bool){
                    throw new ArgumentException("Invalid boolean type argument for filter key", searchFilterRequest.FilterKey);
                }
            });

        _filterExpressionFormatter.SetExpressionParamsSeparator(",");

        return _filterExpressionFormatter
            .CreateFormattedExpression(
                $"search.in({searchFilterRequest.FilterKey}, " +
                $"'{_filterExpressionFormatter.CreateFilterCriteriaPlaceholders(searchFilterRequest.FilterValues)}')",
                searchFilterRequest.FilterValues);
    }
}