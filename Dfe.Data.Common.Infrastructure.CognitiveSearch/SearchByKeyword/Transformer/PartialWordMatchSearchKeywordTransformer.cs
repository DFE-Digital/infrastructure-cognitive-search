using System.Text;

namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword.Transformer;

/// <summary>
/// Facilitates search rules to be specified when running a search
/// </summary>
public sealed class PartialWordMatchSearchKeywordTransformer : ISearchKeywordTransformer
{
    private const char WildcardMatcher = '*';
    /// <summary>
    /// Apply search rules as specified in the options
    /// </summary>
    /// <param name="searchKeyword"></param>
    /// <returns></returns>
    public string Apply(string searchKeyword)
    {
        string normalisedSearchKeyword = searchKeyword.TrimEnd();

        // idempotant
        if (normalisedSearchKeyword.EndsWith(WildcardMatcher))
        {
            return searchKeyword;
        }

        return ApplyWildcardToKeyword(normalisedSearchKeyword);
    }

    private static string ApplyWildcardToKeyword(string input)
    {
        return new StringBuilder(input)
            .Replace(" ", $"{WildcardMatcher} ")
            .Append(WildcardMatcher)
            .ToString();
    }
}
