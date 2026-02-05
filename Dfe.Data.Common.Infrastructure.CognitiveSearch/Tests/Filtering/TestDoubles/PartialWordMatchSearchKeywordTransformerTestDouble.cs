using Dfe.Data.Common.Infrastructure.CognitiveSearch.SearchByKeyword;
using Moq;

namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.Tests.Filtering.TestDoubles;

public static class PartialWordMatchSearchKeywordTransformerTestDouble
{
    public static ISearchKeywordTransformer MockFor(string keywordIn, string keywordOut)
    {
        var mock = new Mock<ISearchKeywordTransformer>();
        mock.Setup(provider => provider.Apply(keywordIn))
            .Returns(keywordOut)
            .Verifiable();
        return mock.Object;
    }
}
