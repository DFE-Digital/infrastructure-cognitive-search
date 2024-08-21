namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.FilterExpressions.Formatters;

/// <summary>
/// 
/// </summary>
public interface IFilterExpressionFormatter
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="separator"></param>
    void SetExpressionParamsSeparator(string separator);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="filterCriteria"></param>
    /// <returns></returns>
    string CreateFilterCriteriaPlaceholders(object[] filterCriteria);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="expressionFormat"></param>
    /// <param name="filterCriteria"></param>
    /// <returns></returns>
    string CreateFormattedExpression(string expressionFormat, params object[] filterCriteria);
}