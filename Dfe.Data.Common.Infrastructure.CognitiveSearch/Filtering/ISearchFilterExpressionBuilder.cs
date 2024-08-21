using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.FilterExpressions.Context;

namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering;

/// <summary>
/// 
/// </summary>
public interface ISearchFilterExpressionBuilder
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="searchFilterContexts"></param>
    /// <returns></returns>
    string BuildSearchFilter(IEnumerable<SearchFilterContext> searchFilterContexts);
}