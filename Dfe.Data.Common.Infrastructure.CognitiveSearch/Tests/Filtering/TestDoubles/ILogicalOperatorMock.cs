using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.LogicalOperators;

namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.Tests.Filtering.TestDoubles;

public static class ILogicalOperators
{
    public static Dictionary<string, Func<ILogicalOperator>> Create()
    {
        return new() {
            { "name1", () => new AndLogicalOperator()}
        };
    }
}
