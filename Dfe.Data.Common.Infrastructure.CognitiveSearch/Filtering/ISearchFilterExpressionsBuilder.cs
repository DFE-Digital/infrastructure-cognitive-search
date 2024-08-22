using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.FilterExpressions.Context;

namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering;

/// <summary>
/// 
/// </summary>
public interface ISearchFilterExpressionsBuilder
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="searchFilterContexts"></param>
    /// <returns></returns>
    string BuildSearchFilterExpressions(IEnumerable<SearchFilterContext> searchFilterContexts);
}