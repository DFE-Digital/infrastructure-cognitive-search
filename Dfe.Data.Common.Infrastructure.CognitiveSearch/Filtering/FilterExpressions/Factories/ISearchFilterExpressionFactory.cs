namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.FilterExpressions.Factories;

/// <summary>
/// 
/// </summary>
public interface ISearchFilterExpressionFactory
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="filterType"></param>
    /// <returns></returns>
    ISearchFilterExpression CreateFilter(Type filterType);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="filterName"></param>
    /// <returns></returns>
    ISearchFilterExpression CreateFilter(string filterName);

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSearchFilterExpression"></typeparam>
    /// <returns></returns>
    ISearchFilterExpression CreateFilter<TSearchFilterExpression>() where TSearchFilterExpression : ISearchFilterExpression;
}