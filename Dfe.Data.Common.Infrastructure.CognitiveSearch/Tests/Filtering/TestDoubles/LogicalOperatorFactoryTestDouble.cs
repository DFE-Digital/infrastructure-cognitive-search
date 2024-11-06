using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.LogicalOperators;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.LogicalOperators.Factories;
using Moq;

namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.Tests.Filtering.FilterExpressions;

internal static class LogicalOperatorFactoryTestDouble
{
    public static Mock<ILogicalOperatorFactory> LogicalOperatorFactoryMock() => new();

    public static ILogicalOperatorFactory MockLogicalOperatorFactory() => MockLogicalOperatorFactoryFor();

    public static ILogicalOperatorFactory MockLogicalOperatorFactoryFor()
    {
        var logicalOperatorFactoryMock = LogicalOperatorFactoryMock();

        logicalOperatorFactoryMock
            .Setup(logicalOperatorFactory =>
                logicalOperatorFactory
                    .CreateLogicalOperator(It.IsAny<string>()))
                        .Returns((string type) =>
                            GetLogicalOperator(type));

        return logicalOperatorFactoryMock.Object;
    }

    public static ILogicalOperator GetLogicalOperator(string logicalOperatorName)
    {
        ILogicalOperator logicalOperator = null!;

        switch (logicalOperatorName)
        {
            case "AndLogicalOperator":
                logicalOperator = new AndLogicalOperator();
                break;
            case "OrLogicalOperator":
                logicalOperator = new OrLogicalOperator();
                break;
        }
        ArgumentNullException.ThrowIfNullOrWhiteSpace(logicalOperatorName);

        return logicalOperator;
    }
}