using System.Text;

namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword.SearchRules;

/// <summary>
/// Facilitates search rules to be specified when running a search
/// </summary>
public sealed class PartialWordMatchRule : ISearchRule
{
    /// <summary>
    /// Apply search rules as specified in the options
    /// </summary>
    /// <param name="searchKeyword"></param>
    /// <returns></returns>
    public string ApplySearchRules(string searchKeyword)
    {
        return new StringBuilder(searchKeyword.TrimEnd())
            .Replace(" ", "* ")
            .Append('*')
            .ToString();
    }
}
