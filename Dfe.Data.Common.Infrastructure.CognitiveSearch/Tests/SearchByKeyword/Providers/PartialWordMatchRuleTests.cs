using Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword.Transformer;
using FluentAssertions;
using Xunit;

namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.Tests.SearchByKeyword.Providers;

public sealed class PartialWordMatchRuleTests
{
    [Theory]
    [InlineData("searchKeyword", "searchKeyword*")]
    [InlineData("searchKeyword ", "searchKeyword*")]
    [InlineData("searchTerm1 searchTerm2", "searchTerm1* searchTerm2*")]
    void ApplySearchKeywordTransformer_WithPartialWordMatchRuleOption_AppliesRule(string searchInput, string expected)
    {
        // arrange

        PartialWordMatchSearchKeywordTransformer provider = new();

        // act
        string searchKeywordResult = provider.Apply(searchInput);

        // assert
        searchKeywordResult.Should().Be(expected);
        searchInput.Should().NotBe(expected);
    }
}
