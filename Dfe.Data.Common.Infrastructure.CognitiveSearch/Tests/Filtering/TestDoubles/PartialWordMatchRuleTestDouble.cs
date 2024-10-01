using Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword;
using Moq;

namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.Tests.Filtering.TestDoubles;

public static class PartialWordMatchRuleTestDouble
{
    public static ISearchRule MockFor(string keywordIn, string keywordOut)
    {
        var mock = new Mock<ISearchRule>();
        mock.Setup(provider => provider.ApplySearchRules(keywordIn))
            .Returns(keywordOut)
            .Verifiable();
        return mock.Object;
    }
}
