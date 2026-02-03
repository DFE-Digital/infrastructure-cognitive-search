using Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword.SearchRules;
using FluentAssertions;
using Xunit;

namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.Tests.SearchByKeyword.Providers;

public sealed class PartialWordMatchRuleTests
{
    [Theory]
    [InlineData("searchKeyword", "searchKeyword*")]
    [InlineData("searchKeyword ", "searchKeyword*")]
    [InlineData("searchTerm1 searchTerm2", "searchTerm1* searchTerm2*")]
    void ApplySearchRules_WithPartialWordMatchRuleOption_AppliesRule(string searchInput, string expected)
    {
        // arrange

        PartialWordMatchRule provider = new();

        // act
        string searchKeywordResult = provider.ApplySearchRules(searchInput);

        // assert
        searchKeywordResult.Should().Be(expected);
        searchInput.Should().NotBe(expected);
    }
}
