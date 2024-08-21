using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.LogicalOperators;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.LogicalOperators.Factories;
using Moq;

namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.Tests.Filtering.FilterExpressions;

internal static class LogicalOperatorFactoryTestDouble
{
    public static Mock<ILogicalOperatorFactory> LogicalOperatorFactoryMock() => new();

    public static ILogicalOperatorFactory MockLogicalOperatorFactory()
    {
        var logicalOperatorFactoryMock = LogicalOperatorFactoryMock();

        _ = logicalOperatorFactoryMock
            .Setup(logicalOperatorFactory =>
                logicalOperatorFactory
                    .CreateLogicalOperator(It.IsAny<string>()))
                        .Returns((string type) =>
                        {
                            ILogicalOperator logicalOperator = null!;

                            switch (type)
                            {
                                case "AndLogicalOperator":
                                    logicalOperator = new AndLogicalOperator();
                                    break;
                                case "OrLogicalOperator":
                                    logicalOperator = new OrLogicalOperator();
                                    break;
                            }

                            return logicalOperator;
                        });

        return logicalOperatorFactoryMock.Object;
    }
}