namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering;

/// <summary>
/// 
/// </summary>
public sealed record SearchFilterRequest
{
    /// <summary>
    /// 
    /// </summary>
    public string FilterKey { get; }
    /// <summary>
    /// 
    /// </summary>
    public object[] FilterValues { get; }

   /// <summary>
   /// 
   /// </summary>
   /// <param name="filterKey"></param>
   /// <param name="filterValues"></param>
   /// <exception cref="ArgumentException"></exception>
    public SearchFilterRequest(string filterKey, IEnumerable<object> filterValues)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filterKey);
        ArgumentNullException.ThrowIfNull(filterValues);

        if (!filterValues.Any())
        {
            throw new ArgumentException(
                "Filter values are required to build search filter arguments", nameof(filterValues));
        }

        FilterKey = filterKey;
        FilterValues = filterValues.ToArray();
    }
}