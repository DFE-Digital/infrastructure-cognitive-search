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
    public string GetFilterExpression(SearchFilterRequest searchFilterContext)
    {
        ArgumentNullException.ThrowIfNull(searchFilterContext);

        // Ensure we only receive a single filter value in the request.
        if (searchFilterContext.FilterValues.Length != 1){
            throw new ArgumentException(
                "Less than or equal to expression expects only one value.", searchFilterContext.FilterKey);
        }

        // Ensure the less than or equal to filter values are in the correct format.
        if (!double.TryParse(searchFilterContext.FilterValues.Single().ToString(), out double number) || number <= 0){
            throw new ArgumentException(
                "Less than or equal to expression must be assigned a positive number or zero.", searchFilterContext.FilterKey);
        }

        return _filterExpressionFormatter
            .CreateFormattedExpression(
                $"le {_filterExpressionFormatter.CreateFilterCriteriaPlaceholders(searchFilterContext.FilterValues)}",
                searchFilterContext.FilterValues);
    }
}
