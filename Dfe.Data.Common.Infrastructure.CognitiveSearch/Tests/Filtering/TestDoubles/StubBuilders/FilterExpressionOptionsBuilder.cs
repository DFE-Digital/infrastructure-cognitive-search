using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.Options;

namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.Tests.Filtering.TestDoubles.StubBuilders
{
    internal class FilterExpressionOptionsBuilder
    {
        private string _filterExpressionKey;
        private string _filterExpressionDelimiterValue;

        public FilterExpressionOptions Create() =>
            new()
            {
                FilterExpressionKey = _filterExpressionKey,
                FilterExpressionValuesDelimiter = _filterExpressionDelimiterValue
            };

        public FilterExpressionOptionsBuilder WithSearchFilterExpressionKey(string filterExpressionKey)
        {
            _filterExpressionKey = filterExpressionKey;
            return this;
        }

        public FilterExpressionOptionsBuilder WithFilterExpressionDelimiterValue(string filterExpressionDelimiterValue)
        {
            _filterExpressionDelimiterValue = filterExpressionDelimiterValue;
            return this;
        }
    }
}
