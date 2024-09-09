using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.FilterExpressions;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.FilterExpressions.Factories;

namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering;

/// <summary>
/// Type used to encapsulate search filter requests by defining the filter key, and
/// associated values used to reconcile to an available OData filter expression
/// <see cref="ISearchFilterExpression"/> type.
/// </summary>
public sealed class SearchFilterRequest
{
    /// <summary>
    /// The key used to reconcile to the configured <see cref="ISearchFilterExpression"/> type
    /// derived using the configured <see cref="ISearchFilterExpressionFactory"/> instance.
    /// </summary>
    public string FilterKey { get; }

    /// <summary>
    /// The values to be applied to the reconciled OData filter expression type.
    /// </summary>
    public object[] FilterValues { get; }

   /// <summary>
   /// Constructor ensures immutability for filter key and filter values provisioned.
   /// </summary>
   /// <param name="filterKey">
   /// The filter key string used to try and reconcile to the actual OData filter expression type.
   /// </param>
   /// <param name="filterValues">
   /// The collection of values to apply to the OData filter expression type.
   /// </param>
   /// <exception cref="ArgumentException">
   /// The exception thrown if no filter values are provisioned.
   /// </exception>
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