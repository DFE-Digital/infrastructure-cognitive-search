namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.FilterExpressions.Factories;

/// <summary>
/// 
/// </summary>
public sealed class SearchFilterExpressionFactory : ISearchFilterExpressionFactory
{
    private readonly Dictionary<string, Func<ISearchFilterExpression>> _filterExpressionfactory;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="filterExpressionFactory"></param>
    public SearchFilterExpressionFactory(Dictionary<string, Func<ISearchFilterExpression>> filterExpressionFactory)
    {
        _filterExpressionfactory = filterExpressionFactory;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSearchFilterExpression"></typeparam>
    /// <returns></returns>
    public ISearchFilterExpression CreateFilter<TSearchFilterExpression>()
        where TSearchFilterExpression : ISearchFilterExpression => CreateFilter(typeof(TSearchFilterExpression));

    /// <summary>
    /// 
    /// </summary>
    /// <param name="filterType"></param>
    /// <returns></returns>
    public ISearchFilterExpression CreateFilter(Type filterType) => CreateFilter(filterName: filterType.Name);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="filterName"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public ISearchFilterExpression CreateFilter(string filterName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filterName);

        return (!_filterExpressionfactory
            .TryGetValue(filterName, out var searchExpressionFilter) || searchExpressionFilter is null) ?
                throw new ArgumentOutOfRangeException(
                    $"Search expression filter of type {filterName} is not registered.") : searchExpressionFilter();
    }
}