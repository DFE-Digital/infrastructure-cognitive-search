namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.Options
{
    /// <summary>
    /// Configuration options for establishing a map to align the incoming filter request
    /// key(s) with an available filter expression and logical operator. For example, we could
    /// create a configuration section as follows,
    /// <code>
    /// "FilterKeyToFilterExpressionMapOptions": {
    ///     "DefaultLogicalOperator": "AndLogicalOperator",
    ///     "SearchFilterToExpressionMap": {
    ///         "RELIGIOUSCHARACTERCODE": "SearchInFilterExpression",
    ///         "OFSTEDRATINGCODE": "SearchInFilterExpression"
    ///     }
    /// }
    /// </code>
    /// If we provide an appropriate set of filter values aligned with the keys 'RELIGIOUSCHARACTERCODE'
    /// and 'OFSTEDRATINGCODE' then the following OData filter expression string should be generated,
    /// <code>
    ///     "search.in(OFSTEDRATINGCODE, '2,5,9,12') and search.in(RELIGIOUSCHARACTERCODE, '00,02')"
    /// </code>
    /// </summary>
    public sealed class FilterKeyToFilterExpressionMapOptions
    {
        /// <summary>
        /// The default logical operator is specified by key as either an <b>AndLogicalOperator</b>
        /// or an <b>OrLogicalOperator</b>. The logical operator is then used to chain together all
        /// search filter expressions specified.
        /// </summary>
        public string? DefaultLogicalOperator { get; set;  }

        /// <summary>
        /// The dictionary used to reconcile the incoming request key with the actual search filter expression to apply.
        /// </summary>
        public IDictionary<string, string> SearchFilterToExpressionMap { get; set; } = new Dictionary<string, string>();
    }
}
