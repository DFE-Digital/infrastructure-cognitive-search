using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.FilterExpressions.Formatters;

namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.FilterExpressions;

/// <summary>
/// Creates an OData comparison operator filter expression which represents the <b>less-than-or-equal-to (le)</b> comparison
/// operator supported by Azure AI search capability. This comparison operator is designed to test whether a field is
/// less than or equal to a constant value. When using the 'le' operator, the assigned value must conform to a numeric
/// format with only a single field to test against. If the value is not provisioned correctly, i.e. there is more than
/// one values passed in a non-numeric format then the appropriate exceptions will be thrown. For further information
/// please visit the following link, <a href="https://learn.microsoft.com/en-us/azure/search/search-query-odata-comparison-operators">
/// OData comparison operators</a>.
/// </summary>
public sealed class LessThanOrEqualToExpression : ISearchFilterExpression
{
    private readonly IFilterExpressionFormatter _filterExpressionFormatter;

    /// <summary>
    /// The <see cref="LessThanOrEqualToExpression"/> uses a <see cref="IFilterExpressionFormatter"/>
    /// to help facilitate the creation of a fully configured 'le' comparison operator filter expression string.
    /// </summary>
    /// <param name="filterExpressionFormatter">
    /// Provides a convenient mechanism for creating composite format strings for use when generating Azure AI OData
    /// filter expressions in string format.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Exception thrown if a null <see cref="IFilterExpressionFormatter"/> is injected.
    /// </exception>
    public LessThanOrEqualToExpression(IFilterExpressionFormatter filterExpressionFormatter)
    {
        _filterExpressionFormatter =
            filterExpressionFormatter ??
            throw new ArgumentNullException(nameof(filterExpressionFormatter));
    }

    /// <summary>
    /// Creates the formatted <b>less-than-or-equal-to (le)</b> comparison operator filter expression string based on
    /// the requirements specified in the <see cref="SearchFilterRequest"/> object. Validation is included to
    /// ensure the expression is configured correctly, i.e. the filter request value is expected to represent
    /// a numeric value, and only a single value is expected that must parse correctly to a double.
    /// </summary>
    /// <param name="searchFilterRequest">
    /// The <see cref="SearchFilterRequest"/> object carries the filter and values used to configure the filter expression.
    /// </param>
    /// <returns>
    /// A configured <b>less-than-or-equal-to (le)</b> OData Azure AI filter expression in string format.
    /// </returns>
    public string GetFilterExpression(SearchFilterRequest searchFilterRequest)
    {
        ArgumentNullException.ThrowIfNull(searchFilterRequest);

        // Ensure we only receive a single filter value in the request.
        if (searchFilterRequest.FilterValues.Length != 1){
            throw new ArgumentException(
                "Less than or equal to expression expects only one value.", searchFilterRequest.FilterKey);
        }

        // Ensure the less than or equal to filter values are in the correct format.
        if (!double.TryParse(searchFilterRequest.FilterValues.Single().ToString(), out double number) || number <= 0){
            throw new ArgumentException(
                "Less than or equal to expression must be assigned a positive number or zero.", searchFilterRequest.FilterKey);
        }

        return _filterExpressionFormatter
            .CreateFormattedExpression(
                $"le {_filterExpressionFormatter.CreateFilterCriteriaPlaceholders(searchFilterRequest.FilterValues)}",
                searchFilterRequest.FilterValues);
    }
}
