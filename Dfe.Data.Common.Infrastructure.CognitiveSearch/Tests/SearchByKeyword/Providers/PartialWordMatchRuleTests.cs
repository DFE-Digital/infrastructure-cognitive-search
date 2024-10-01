using Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword.Options;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword.SearchRules;
using FluentAssertions;
using Xunit;

namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.Tests.SearchByKeyword.Providers;

public class PartialWordMatchRuleTests
{
    [Theory]
    [InlineData("searchKeyword", "searchKeyword*")]
    [InlineData("searchKeyword ", "searchKeyword*")]
    [InlineData("searchTerm1 searchTerm2", "searchTerm1* searchTerm2*")]
    void ApplySearchRules_WithPartialWordMatchRuleOption_AppliesRule(string searchInput, string expected)
    {
        // arrange
        var searchRulesOptions = new SearchRuleOptions()
        {
            SearchRule = "PartialWordMatch"
        };

        var provider = new CognitiveSearch.SearchByKeyword.SearchRules.PartialWordMatchRule(searchRulesOptions);

        // act
        var searchKeywordResult = provider.ApplySearchRules(searchInput);

        // assert
        searchKeywordResult.Should().Be(expected);
        searchInput.Should().NotBe(expected);
    }

    [Fact]
    void ApplySearchRules_WithNotApplicableRuleOption_ReturnsUnmodified()
    {
        // arrange
        var searchRulesOptions = new SearchRuleOptions()
        {
            SearchRule = "a nonexistent rule"
        };

        var provider = new PartialWordMatchRule(searchRulesOptions);

        // act
        var searchKeywordResult = provider.ApplySearchRules("searchKeyword");

        // assert
        searchKeywordResult.Should().Be("searchKeyword");
    }

    [Fact]
    void ApplySearchRules_WithNoOption_ReturnsUnmodified()
    {
        // arrange
        var searchRulesOptions = new SearchRuleOptions();

        var provider = new PartialWordMatchRule(searchRulesOptions);

        // act
        var searchKeywordResult = provider.ApplySearchRules("searchKeyword");

        // assert
        searchKeywordResult.Should().Be("searchKeyword");
    }
}
