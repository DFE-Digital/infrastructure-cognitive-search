using System.Runtime.CompilerServices;
using System.Text;

namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.FilterExpressions.Formatters;

/// <summary>
/// 
/// </summary>
public sealed class DefaultFilterExpressionFormatter : IFilterExpressionFormatter
{
    /// <summary>
    /// 
    /// </summary>
    private string ExpressionParamsSeparator { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="separator"></param>
    public void SetExpressionParamsSeparator(string separator) { ExpressionParamsSeparator = separator; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="expressionFormat"></param>
    /// <param name="filterCriteria"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public string CreateFormattedExpression(string expressionFormat, params object[] filterCriteria)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(expressionFormat);

        if (filterCriteria.Length == 0){
            throw new ArgumentException(
                "Filter argument cannot be null or empty", nameof(filterCriteria));
        }

        return FormattableStringFactory.Create(expressionFormat, filterCriteria).ToString();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="filterCriteria"></param>
    /// <returns></returns>
    public string CreateFilterCriteriaPlaceholders(object[] filterCriteria)
    {
        StringBuilder filterCriteriaFormat = new();

        filterCriteriaFormat
            .AppendJoin(
                ExpressionParamsSeparator,
                Enumerable.Range(0, filterCriteria.Length).Select(placeholder => $"{{{placeholder}}}"));

        return filterCriteriaFormat.ToString();
    }
}
