using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.Options;

namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.Tests.Filtering.TestDoubles.StubBuilders
{
    internal class FilterKeyToFilterExpressionMapOptionsBuilder
    {
        private string _filterChainingLogicalOperatorKey = "AndLogicalOperator";
        private IDictionary<string, string> _searchFilterToExpressionMap = new Dictionary<string, string>();

        public FilterKeyToFilterExpressionMapOptions Create() =>
            new()
            {
                SearchFilterToExpressionMap = _searchFilterToExpressionMap,
                FilterChainingLogicalOperator = _filterChainingLogicalOperatorKey,
            };

        public FilterKeyToFilterExpressionMapOptionsBuilder WithSearchFilterToExpressionMap(IDictionary<string, string> searchFilterToExpressionMap)
        {
            _searchFilterToExpressionMap = searchFilterToExpressionMap;
            return this;
        }

        public FilterKeyToFilterExpressionMapOptionsBuilder WithFilterChainingLogicalOperator(string filterChainingLogicalOperatorKey)
        {
            _filterChainingLogicalOperatorKey = filterChainingLogicalOperatorKey;
            return this;
        }
    }
}
