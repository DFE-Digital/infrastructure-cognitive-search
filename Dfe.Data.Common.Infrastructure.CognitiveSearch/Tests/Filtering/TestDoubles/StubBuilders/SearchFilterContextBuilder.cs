using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering;

namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.Tests.Filtering.FilterExpressions.StubBuilders;

public sealed class SearchFilterRequestBuilder
{
    private string _filterKey = null!;
    private readonly List<object?> _filterValues = new();
    private readonly List<SearchFilterRequest> _searchFilterRequests = new();

    public SearchFilterRequestBuilder BuildSearchFilterRequestsWith(params (string, List<object>?)[] searchFilterRequestParams)
    {
        foreach (var searchFilterRequestParam in searchFilterRequestParams)
        {
            _searchFilterRequests.Add(
                Create()
                .WithFilterKey(searchFilterRequestParam.Item1)
                .WithFilterValues(searchFilterRequestParam.Item2).Build());
        }

        return this;
    }

    public SearchFilterRequestBuilder WithFilterKey(string filterKey)
    {
        _filterKey = filterKey;
        return this;
    }

    public SearchFilterRequestBuilder WithFilterValues(List<object>? filterValues)
    {
        filterValues?.ForEach(filterValue =>
            WithFilterValue(filterValue));

        return this;
    }

    public SearchFilterRequestBuilder WithFilterValue(object? filterValue)
    {
        _filterValues.Add(filterValue);
        return this;
    }

    public static SearchFilterRequestBuilder Create() => new();

    public SearchFilterRequest Build() => new(_filterKey, _filterValues!);
    public List<SearchFilterRequest> BuildSearchFilterRequests() => _searchFilterRequests;
}
