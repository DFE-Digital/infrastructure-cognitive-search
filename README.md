
# infrastructure-cognitive-search

A library to provide an accessible API for working with Azure cognitive search. The package contains a fully configured default service for searching by keyword, as well as a geo-location service which allows searches to be made by town, or post-code. The package is intended to take the heavy-lifting away in terms of setup and configurartion and allow for an easy, pluggable set of components that can be used across projects. 

## Getting Started

The dependencies can be used in isolation or registered as a whole under a single composition root. For example, you could opt to use the GeoLocationClientProvider and create your own concrete geo-location service implementation, rather than using the default provided. 

### Prerequisites

In order to use the default search services it is possible to register all dependencies listed under the default composition root, in one registration, as follows:

```csharp
builder.Services.AddDefaultCognitiveSearchServices(builder.Configuration);
```

Alternatively, the registrations can be configured in the consuming application's IOC container, with a typical registration configured similar to the following:

```csharp
services.TryAddSingleton<ISearchByKeywordClientProvider, SearchByKeywordClientProvider>();
services.TryAddSingleton<ISearchIndexNamesProvider, SearchIndexNamesProvider>();
services.TryAddSingleton<ISearchByKeywordService, DefaultSearchByKeywordService>();
services.TryAddScoped<IGeoLocationClientProvider, GeoLocationClientProvider>();
services.TryAddScoped<IGeoLocationService, DefaultGeoLocationService>();

services.AddOptions<SearchByKeywordClientOptions>()
   .Configure<IConfiguration>(
	   (settings, configuration) =>
		   configuration
			   .GetSection(nameof(SearchByKeywordClientOptions))
			   .Bind(settings));

services.AddOptions<GeoLocationOptions>()
   .Configure<IConfiguration>(
	   (settings, configuration) =>
		   configuration
			   .GetSection(nameof(GeoLocationOptions))
			   .Bind(settings));

services.AddHttpClient("GeoLocationHttpClient", config =>
{
	var geoLocationOptions =
		configuration
			.GetSection(nameof(GeoLocationOptions)).Get<GeoLocationOptions>();

	ArgumentNullException.ThrowIfNull(geoLocationOptions);
	ArgumentNullException.ThrowIfNullOrWhiteSpace(geoLocationOptions.MapsServiceUri);

	config.BaseAddress = new Uri(geoLocationOptions.MapsServiceUri);
	config.Timeout =
		new TimeSpan(
			geoLocationOptions.RequestTimeOutHours,
			geoLocationOptions.RequestTimeOutMinutes,
			geoLocationOptions.RequestTimeOutSeconds);

	config.DefaultRequestHeaders.Clear();
});
```

### Code Usage/Examples

Typical dependency injection and search request would look something like the following,

```csharp
public sealed class CognitiveSearchServiceAdapter<TSearchResult> : ISearchServiceAdapter where TSearchResult : class
{
    private readonly ISearchService _cognitiveSearchService;
    private readonly ISearchOptionsFactory _searchOptionsFactory;
    private readonly IMapper<Response<SearchResults<TSearchResult>>, EstablishmentResults> _searchResponseMapper;

    public CognitiveSearchServiceAdapter(
        ISearchService cognitiveSearchService,
        ISearchOptionsFactory searchOptionsFactory,
        IMapper<Response<SearchResults<TSearchResult>>, EstablishmentResults> searchResponseMapper)
    {
        _searchOptionsFactory = searchOptionsFactory;
        _cognitiveSearchService = cognitiveSearchService;
        _searchResponseMapper = searchResponseMapper;
    }

    public async Task<EstablishmentResults> SearchAsync(SearchContext searchContext)
    {
        SearchOptions searchOptions =
            _searchOptionsFactory.GetSearchOptions(searchContext.TargetCollection) ??
            throw new ApplicationException(
                $"Search options cannot be derived for {searchContext.TargetCollection}.");

        Response<SearchResults<TSearchResult>> searchResults =
            await _cognitiveSearchService.SearchAsync<TSearchResult>(
                searchContext.SearchKeyword,
                searchContext.TargetCollection,
                searchOptions
            )
            .ConfigureAwait(false) ??
                throw new ApplicationException(
                    $"Unable to derive search results based on input {searchContext.SearchKeyword}.");

        return _searchResponseMapper.MapFrom(searchResults);
    }
}
```

## Built With

* [.Net 8](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-8/overview) - Core framework used
* [Azure](https://learn.microsoft.com/en-us/azure/search/) - Cloud services provider (cognitive search)


## Versioning

We use [SemVer](http://semver.org/) for versioning. For the versions available, see the [tags on this repository](https://github.com/DFE-Digital/infrastructure-cognitive-search/tags). 

## Authors

* **Spencer O'Hegarty** - *Initial work*
* **Catherine Lawlor**
* **Asia Witek**

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details
