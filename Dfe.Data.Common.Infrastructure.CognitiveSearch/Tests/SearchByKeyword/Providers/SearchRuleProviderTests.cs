﻿using Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword.Options;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword.Providers;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.Tests.SearchByKeyword.Providers;

public class SearchRuleProviderTests
{
    [Fact]
    void ApplySearchRules_WithPartialWordMatchRuleOption_AppliesRule()
    {
        // arrange
        var searchRulesOptions = new SearchRuleOptions()
        {
            SearchRule = "PartialWordMatch"
        };

        var provider = new SearchRuleProvider(searchRulesOptions);

        // act
        var searchKeywordResult = provider.ApplySearchRules("searchKeyword");

        // assert
        searchKeywordResult.Should().Be("searchKeyword*");
    }

    [Fact]
    void ApplySearchRules_WithNotApplicableRuleOption_ReturnsUnmodified()
    {
        // arrange
        var searchRulesOptions = new SearchRuleOptions()
        {
            SearchRule = "a nonexistent rule"
        };

        var provider = new SearchRuleProvider(searchRulesOptions);

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

        var provider = new SearchRuleProvider(searchRulesOptions);

        // act
        var searchKeywordResult = provider.ApplySearchRules("searchKeyword");

        // assert
        searchKeywordResult.Should().Be("searchKeyword");
    }
}
