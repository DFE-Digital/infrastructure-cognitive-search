namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.FilterExpressions;

/// <summary>
/// Provides an abstraction over which to derive Azure AI OData filter expressions.
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