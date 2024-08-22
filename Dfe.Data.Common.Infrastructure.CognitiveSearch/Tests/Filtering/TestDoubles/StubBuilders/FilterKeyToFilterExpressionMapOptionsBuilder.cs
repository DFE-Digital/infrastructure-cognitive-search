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

    #region TO BE REMOVED
    //private SearchFilterKeyToFilterExpressionMap GetFilterExpressionMap()
    //{
    //    var searchFilterToExpressionMap = new Dictionary<string, string>()
    //    {
    //        { "OFSTEDRATINGCODE", "SearchInFilterExpression"},
    //        { "RELIGIOUSCHARACTERCODE", "SearchInFilterExpression" },
    //        { "GEODISTANCE", "LessThanOrEqualToExpression" },
    //        { "GEOLOCATION", "SearchGeoLocationFilterExpression" }
    //    };

    //    return new SearchFilterKeyToFilterExpressionMap("AndLogicalOperator", searchFilterToExpressionMap);
    //}

    //internal class SearchFilterKeyToFilterExpressionMap
    //{
    //    public SearchFilterKeyToFilterExpressionMap(
    //        string defaultLogicalOperator,
    //        IDictionary<string, string> searchFilterToExpressionMap)
    //    {
    //        DefaultLogicalOperator = defaultLogicalOperator;
    //        SearchFilterToExpressionMap = searchFilterToExpressionMap;
    //    }

    //    public string DefaultLogicalOperator { get; }

    //    public IDictionary<string, string> SearchFilterToExpressionMap { get; }
    //}
    #endregion
}
