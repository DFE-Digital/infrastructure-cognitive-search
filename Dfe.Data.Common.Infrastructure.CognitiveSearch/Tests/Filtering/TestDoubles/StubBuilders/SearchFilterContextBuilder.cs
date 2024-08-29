using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering;

namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.Tests.Filtering.FilterExpressions.StubBuilders;

public sealed class SearchFilterContextBuilder
{
    private string _facet = null!;
    private readonly List<object?> _facetedValues = new();
    private readonly List<SearchFilterRequest> _searchFilterContexts = new();

    public SearchFilterContextBuilder BuildSearchFilterContextsWith(params (string, List<object>?)[] searchFilterContextParams)
    {
        foreach (var searchFilterContextParam in searchFilterContextParams)
        {
            _searchFilterContexts.Add(
                Create()
                .WithFacetKey(searchFilterContextParam.Item1)
                .WithFacetedValues(searchFilterContextParam.Item2).Build());
        }

        return this;
    }

    public SearchFilterContextBuilder WithFacetKey(string facetKey)
    {
        _facet = facetKey;
        return this;
    }

    public SearchFilterContextBuilder WithFacetedValues(List<object>? facetedValues)
    {
        facetedValues?.ForEach(facetedValue =>
            WithFacetedValue(facetedValue));

        return this;
    }

    public SearchFilterContextBuilder WithFacetedValue(object? facetedValue)
    {
        _facetedValues.Add(facetedValue);
        return this;
    }

    public static SearchFilterContextBuilder Create() => new();

    public SearchFilterRequest Build() => new(_facet, _facetedValues!);
    public List<SearchFilterRequest> BuildSearchFilterContexts() => _searchFilterContexts;
}
