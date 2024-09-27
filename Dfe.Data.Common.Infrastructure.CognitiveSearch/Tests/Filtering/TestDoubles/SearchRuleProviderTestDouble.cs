using Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword.Providers;
using Moq;

namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.Tests.Filtering.TestDoubles;

public static class SearchRuleProviderTestDouble
{
    public static ISearchRuleProvider MockFor(string keyword)
    {
        var mock = new Mock<ISearchRuleProvider>();
        mock.Setup(x => x.ApplySearchRules(keyword))
            .Returns(keyword)
            .Verifiable();
        return mock.Object;
    }
}
