using Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword.Transformer;
using FluentAssertions;
using Xunit;

namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.Tests.SearchByKeyword.Transformers;

public sealed class PartialWordMatchSearchKeywordTransformerTests
{
    [Theory]
    [InlineData("", "*")]
    [InlineData(" ", "*")]
    [InlineData("searchKeyword", "searchKeyword*")]
    [InlineData("searchKeyword ", "searchKeyword*")]
    [InlineData("searchTerm1 searchTerm2", "searchTerm1* searchTerm2*")]
    public void ApplySearchKeywordTransformer_WithPartialWordMatchRuleOption_AppliesRule(string searchInput, string expected)
    {
        // arrange
        PartialWordMatchSearchKeywordTransformer transformer = new();

        // act
        string searchKeywordResult = transformer.Apply(searchInput);

        // assert
        searchKeywordResult.Should().Be(expected);
        searchInput.Should().NotBe(expected);
    }

    [Theory]
    [InlineData("searchKeyword *")]
    [InlineData(" searchKeyword * ")]
    [InlineData("searchKeyword*")]
    public void ApplySearchKeywordTransformer_WithPartialWordMatchTransformer_DoesNotApply_If_AlreadyApplied(string input)
    {
        // arrange
        PartialWordMatchSearchKeywordTransformer transformer = new();

        // act
        string searchKeywordResult = transformer.Apply(input);

        // assert
        Assert.Same(input, searchKeywordResult);
    }
}
