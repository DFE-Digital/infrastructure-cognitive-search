﻿using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;

namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword;

/// <summary>
/// 
/// </summary>
public interface ISearchByKeywordService
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSearchResult"></typeparam>
    /// <param name="searchKeyword"></param>
    /// <param name="searchIndex"></param>
    /// <param name="searchOptions"></param>
    /// <returns></returns>
    Task<Response<SearchResults<TSearchResult>>> SearchAsync<TSearchResult>(
        string searchKeyword, string searchIndex, SearchOptions searchOptions)
        where TSearchResult : class;
}