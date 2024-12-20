
# infrastructure-cognitive-search

A library to provide an accessible API for working with Azure AI search and Azure maps search. 
The package contains a fully configured default service for searching by keyword, as well as a Azure maps search service which allows searches to 
be made by town, or post-code. The package is intended to take the heavy-lifting away in terms of setup and configuration and allow for an easy, 
pluggable set of components that can be used across projects. 

## Getting Started

The dependencies can be used in isolation or registered as using the extension methods provided via the composition root. 
For example, you could opt to use the GeoLocationClientProvider and create your own concrete geo-location service implementation, rather than using the default provided. 

Search service dependencies can be registered using the extension methods provided:

```csharp
builder.Services.AddAzureSearchServices(builder.Configuration);
builder.Services.AddAzureSearchFilterServices(builder.Configuration);
builder.Services.AddAzureGeoLocationSearchServices(builder.Configuration);
```

## Execute a search against Azure AI search
```csharp
builder.Services.AddAzureSearchServices(builder.Configuration);
```
Registers the basic functionality to send a search request to Azure AI Search.
This includes the following services:

- An implementation of `ISearchByKeywordService` which is the main class to use to submit a search request to Azure AI search
- An implementation of `ISearchByKeywordClientProvider` which is used to create an instance of the search client to connect to Azure AI search.
It requires the following appsettings:
```json
{
  "AzureSearchConnectionOptions": {
    "EndpointUri": "https://your-search-service-name.search.windows.net/",
    "Credentials": "your-search-service-api-key - served from Azure key vault or other"
  }
}
```
### Basic usage
```csharp
public async Task<Response<SearchResults<TSearchResult>>> UseSearchService(ISearchByKeywordService searchService, string searchKeyword, string indexName, SearchOptions searchOptions)
{
    return await searchService.SearchAsync(searchKeyword, indexName, searchOptions);
}
```
where 
```search-keyword``` is the keyword to search for,
```index-name``` is the name of the index in Azure AI search to search in and
`searchOptions` is the object of type [SearchOptions](https://learn.microsoft.com/en-us/dotnet/api/azure.search.documents.searchoptions?view=azure-dotnet&devlangs=csharp&f1url=%3FappId%3DDev17IDEF1%26l%3DEN-US%26k%3Dk(Azure.Search.Documents.SearchOptions)%3Bk(DevLang-csharp)%26rd%3Dtrue) 
that specifies the search request to be submitted.

## Add filtering to search

Filtering a search can be accomplished using only the simple search service explained above and by formatting the ```Filter``` property of the ```SearchOptions``` object.
However, the SearchFilterServices provides additional services to facilitate the construction of the filter expression used by the Azure AI search API.
```csharp
builder.Services.AddAzureSearchFilterServices(builder.Configuration);
```

This includes the following services:
- An implementation of ```ISearchFilterExpressionsBuilder``` which co-ordinates the build of the filter expression given the config (explained below)
- Three implementations of ```ISearchFilterExpression``` - ```SearchInFilterExpression```, ```LessThanOrEqualToExpression```, ```SearchGeoLocationFilterExpression```
- Two implementations of ```ILogicalOperator``` - ```AndLogicalOperator```, ```OrLogicalOperator``` that determine how the filters are combined if more than one filter field is used
These interfaces can be extended with your own custom implementations to add more filter expressions and logical operators as needed.

To use the filter services, you must configure the filter fields and their settings in appsettings. For example, the following appsettings 
show a filter field ```PHASEOFEDUCATION``` specified to use the odata ```Search.in``` expression when filter values are applied to this field.
```json
{
  "FilterKeyToFilterExpressionMapOptions": {
    "SearchFilterToExpressionMap": {
      "PHASEOFEDUCATION": {
        "FilterExpressionKey": "SearchInFilterExpression",
        "FilterExpressionValuesDelimiter": ","
      }
    }
  }
}
```
When a call is made to the ```BuildSearchFilterExpressions``` method of the ISearchFilterExpressionsBuilder, the filter expression is built using the filter expression key and the filter values. 
The filter values are split by the ```FilterExpressionValuesDelimiter``` specified in the appsettings. For example, the following code snippet shows how the filter expression is built using the filter values "Primary", "Secondary":
```csharp
_searchOptions.Filter =
    _searchFilterExpressionsBuilder.BuildSearchFilterExpressions(
        new SearchFilterRequest("PHASEOFEDUCATION", new List<string>(){"Primary", "Secondary"}));
```

The result is the odata filter expression 
```"search.in(PHASEOFEDUCATION, 'Primary,Secondary', ',')"```
which can be assigned directly to the Azure SearchOptions.Filter property

When more than one filter field is to be used, the filter expression can include chained filters using any of the 
implementations of ```ILogicalOperator``` by adding the ```FilterChainingLogicalOperator``` property to appsettings:
```json
{
  "FilterKeyToFilterExpressionMapOptions": {
    "SearchFilterToExpressionMap": {
      "FilterChainingLogicalOperator": "AndLogicalOperator"
      }
   }
}
```

## Azure maps search
```csharp
builder.Services.AddAzureGeoLocationSearchServices(builder.Configuration);
```
Registers the basic functionality to send a location search request to the [Azure maps search address API](https://learn.microsoft.com/en-us/rest/api/maps/search/get-search-address?view=rest-maps-1.0&tabs=HTTP).
It registers the following services:

- An implementation of `IGeoLocationService` which is the main class to use to submit a search request to the Azure maps search API
- An implementation of `IGeoLocationClientProvider` which is used to create an instance of the search client to connect to Azure maps search.

It requires the following appsettings:
```json
{
  "AzureGeoLocationOptions": {
    "MapsServiceUri": "https://atlas.microsoft.com/",
    "SearchEndpointUri": "<your endpoint url in the format specified below",
    "MapsSubscriptionKey": "<your maps subscription key - served from Azure Key Vault or other>"
  }
}
```
```SearchEndpointUri``` should be in the format 
```search/address/json?api-version=1.0&countrySet=GB&typeahead=true&limit=10&query={0}&subscription-key={1}"```

### Basic usage
```csharp
 GeoLocationServiceResponse? response =
                await geoLocationService?.SearchGeoLocationAsync("<location string>")!;
```

## Custom registration
Instead of using the extension methods to register the required services, the registrations can be configured manually in the consuming application's IOC container, with a typical registration configured similar to the following:

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

## Built With

* [.Net 8](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-8/overview) - Core framework used
* [Azure](https://learn.microsoft.com/en-us/azure/search/) - Cloud services provider (cognitive search)

## Versioning

We use [SemVer](http://semver.org/) for versioning. For the versions available, see the [tags on this repository](https://github.com/DFE-Digital/infrastructure-cognitive-search/tags). 

## Using The Nuget Packages From Your Development Machine
The Nuget packages provided by this repository are served under the DfE-Digital organisation.
The be able to use these Nuget Packages (and others) you must configure your development machine to have a new NuGet Package Source.
To do this, you must first create a PAT token that has at least __read access for packages__.

> **NEVER commit your PAT token to GitHub or any other VCS !**

Next add a package source to your NuGet configuration using the CLI.
Use the following command, replacing `[USERNAME] with your GitHub username, and `[PAT-TOKEN] with the PAT token you just generated.

`dotnet nuget add source --username "[USERNAME]" --password "[PAT-TOKEN]" --store-password-in-clear-text --name DfE "https://nuget.pkg.github.com/DFE-Digital/index.json"`

> Alternatively you may add a package source directly in Visual Studio.Once you have generated a PAT token you can add a new NuGet Package Source to visual studio. You may be prompted to sign in, if you are then enter your GitHub username and instead of the password enter the PAT token you generated.

---
 
## Referencing the Nuget Registry From a GitHub Action That Directly Builds DotNet Projects
This applies when building dotnet solutions that reference the nuget registry directly within a GitHub action.

You can use the Nuget Registry from a GitHub action pipeline without need for a PAT token. GitHub creates a special token for use during the lifetime of the GitHub action. For your apps to be able to restore from the DfE nuget repository, add the followint two lines to your yml file __before__ restoring packages

```sh
- name: Add nuget package source
  run: dotnet nuget add source --username USERNAME --password ${{ secrets.GITHUB_TOKEN }} --store-password-in-clear-text --name github "https://nuget.pkg.github.com/DFE-Digital/index.json"
```

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details
