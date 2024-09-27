using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword.Options;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword.Providers;
using Microsoft.Extensions.Options;

namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword;

/// <summary>
/// The default service provisioned to allow requests to made to the azure search services,
/// by keyword, to provide a consistent, configured response based on the status of the request work-flow.
/// </summary>
public sealed class DefaultSearchByKeywordService : ISearchByKeywordService
{
    private readonly ISearchByKeywordClientProvider _searchClientProvider;
    private readonly SearchRuleOptions _ruleOptions;

    /// <summary>
    /// The following T:Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword.Providers.ISearchByKeywordClientProvider
    /// dependency Provides a contract by which to derive (by index name) a configured T:Azure.Search.Documents.SearchClient
    /// for use when making search by key-word requests.
    /// </summary>
    /// <param name="searchClientProvider">
    /// The T:Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword.Providers.ISearchByKeywordClientProvider instance
    /// used to provision a configured Azure search client provider.
    /// </param>
    /// <param name="searchRuleOptions">
    /// The <see cref="SearchRuleOptions"/> that specify details of how the search should be performed 
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// The exception thrown when an attempt is made to inject a null search client provider.
    /// </exception>
    public DefaultSearchByKeywordService(
        ISearchByKeywordClientProvider searchClientProvider,
        SearchRuleOptions searchRuleOptions)
    {
        _searchClientProvider = searchClientProvider ??
            throw new ArgumentNullException(nameof(searchClientProvider));
        _ruleOptions = searchRuleOptions;
    }

    /// <summary>
    /// Makes a call to the underlying azure search service client using
    /// the prescribed search keyword and options request, and returns a
    /// response which aggregate the search result(s), if nay.
    /// </summary>
    /// <typeparam name="TSearchResult">
    /// The type of search result into which to unpack the raw search response.
    /// </typeparam>
    /// <param name="searchKeyword">
    /// The string search keyword used across the prescribed index.
    /// </param>
    /// <param name="searchIndex">
    /// The name of the index on which to assign the search.
    /// </param>
    /// <param name="searchOptions">
    /// Options for <see cref="SearchClient.SearchAsync"/> that
    /// allow specifying filtering, sorting, faceting, paging, and other search
    /// query behaviors.
    /// </param>
    /// <returns>
    /// The response containing search results from an index.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the param <paramref name="searchKeyword"/> or the param <paramref name="searchIndex"/> are null or empty
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the param <paramref name="searchOptions"/>is null
    /// </exception>
    /// <exception cref="RequestFailedException">
    /// Thrown when a failure is returned by the Azure Search Service
    /// </exception>
    public Task<Response<SearchResults<TSearchResult>>> SearchAsync<TSearchResult>(
        string searchKeyword, string searchIndex, SearchOptions searchOptions)
        where TSearchResult : class
    {
        ArgumentException.ThrowIfNullOrEmpty(searchKeyword);
        ArgumentException.ThrowIfNullOrEmpty(searchIndex);
        ArgumentNullException.ThrowIfNull(searchOptions);


        return InvokeSearch(
            searchIndex, (searchClient) =>
                searchClient.SearchAsync<TSearchResult>(
                    searchKeyword, searchOptions).Result);
    }

    /// <summary>
    /// Makes a call to the underlying search client provider to invoke a search
    /// using the delegated search action, across the specified index.
    /// </summary>
    /// <typeparam name="TResult">
    /// The type of search result into which to unpack the raw search response.
    /// </typeparam>
    /// <param name="searchIndex">
    /// The name of the index on which to assign the search.
    /// </param>
    /// <param name="searchAction">
    /// The delegate which encapsulates the search action to invoke on the
    /// continuation of the asynchronous search client invocation.
    /// </param>
    /// <returns>
    /// The type of search result into which to unpack the raw search response.
    /// </returns>
    private Task<TResult> InvokeSearch<TResult>(
        string searchIndex, Func<SearchClient, TResult> searchAction) where TResult : class
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(searchIndex);

        return _searchClientProvider
            .InvokeSearchClientAsync(searchIndex)
            .ContinueWith(
                searchClient =>
                    searchAction(searchClient.Result),
                TaskContinuationOptions.OnlyOnRanToCompletion);
    }
}
