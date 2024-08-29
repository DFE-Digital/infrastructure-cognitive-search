using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.FilterExpressions.Context;

namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.FilterExpressions;

/// <summary>
/// 
/// </summary>
public interface ISearchFilterExpression
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="searchFilterContext"></param>
    /// <returns></returns>
    string GetFilterExpression(SearchFilterRequest searchFilterContext);
}