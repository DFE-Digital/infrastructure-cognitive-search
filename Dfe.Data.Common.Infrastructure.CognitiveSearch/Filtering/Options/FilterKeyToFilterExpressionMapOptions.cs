namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.Options
{
    public sealed class FilterKeyToFilterExpressionMapOptions
    {
        /// <summary>
        /// 
        /// </summary>
        public string? DefaultLogicalOperator { get; set;  }

        /// <summary>
        /// 
        /// </summary>
        public IDictionary<string, string> SearchFilterToExpressionMap { get; set; } = new Dictionary<string, string>();
    }
}
