namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword.IndexNames.Options;
/// <summary>
/// 
/// </summary>
public sealed class SearchIndexNamesOptions
{
    /// <summary>
    /// Rules for Azure Search index names https://learn.microsoft.com/en-us/rest/api/searchservice/naming-rules 
    /// </summary>
    public List<string>? Names { get; set; }
}
