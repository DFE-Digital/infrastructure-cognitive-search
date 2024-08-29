using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.Options;

namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.Tests.Filtering.TestDoubles.StubBuilders
{
    internal class FilterKeyToFilterExpressionMapOptionsBuilder
    {
        private string _defaultLogicalOperatorKey = "AndLogicalOperator";
        private IDictionary<string, string> _searchFilterToExpressionMap = new Dictionary<string, string>();

        public FilterKeyToFilterExpressionMapOptions Create() =>
            new()
            {
                SearchFilterToExpressionMap = _searchFilterToExpressionMap,
                DefaultLogicalOperator = _defaultLogicalOperatorKey,
            };

        public FilterKeyToFilterExpressionMapOptionsBuilder WithSearchFilterToExpressionMap(IDictionary<string, string> searchFilterToExpressionMap)
        {
            _searchFilterToExpressionMap = searchFilterToExpressionMap;
            return this;
        }

        public FilterKeyToFilterExpressionMapOptionsBuilder WithDefaultLogicalOperator(string defaultLogicalOperatorKey)
        {
            _defaultLogicalOperatorKey = defaultLogicalOperatorKey;
            return this;
        }
    }
}
