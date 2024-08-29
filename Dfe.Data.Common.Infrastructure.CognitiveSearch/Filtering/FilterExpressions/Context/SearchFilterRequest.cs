namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.FilterExpressions.Context;

/// <summary>
/// 
/// </summary>
public sealed class SearchFilterRequest
{
    /// <summary>
    /// 
    /// </summary>
    public string Facet { get; }
    /// <summary>
    /// 
    /// </summary>
    public object[] FacetedValues { get; }

    /// <summary>
    /// s
    /// </summary>
    /// <param name="facet"></param>
    /// <param name="facetedValues"></param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public SearchFilterRequest(string facet, IEnumerable<object> facetedValues)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(facet);
        ArgumentNullException.ThrowIfNull(facetedValues);

        if (!facetedValues.Any())
        {
            throw new ArgumentException(
                "Faceted values are required to build search filter arguments", nameof(facetedValues));
        }

        Facet = facet;
        FacetedValues = facetedValues.ToArray();
    }
}