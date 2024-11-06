using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.LogicalOperators;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.LogicalOperators.Factories;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.Tests.Filtering.TestDoubles;
using FluentAssertions;
using Xunit;

namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.Tests.Filtering.LogicalOperators.Factories;

public sealed class LogicalOperatorFactoryTests
{
    [Fact]
    public void CreateLogicalOperator_WithValidOperatorName_ReturnsExpectedConfiguredOperatorFromfactory()
    {
        // arrange.
        ILogicalOperatorFactory logicalOperatorFactory = new LogicalOperatorFactory(ILogicalOperators.Create());

        // act.
        ILogicalOperator logicalOperator =
            logicalOperatorFactory.CreateLogicalOperator("name1");

        // assert.
        logicalOperator.Should().NotBeNull();
        logicalOperator.Should().BeOfType<AndLogicalOperator>();
    }

    [Fact]
    public void CreateFilter_WithUnknownFilterName_ThrowsExpectedException()
    {
        // arrange.
        ILogicalOperatorFactory logicalOperatorFactory = new LogicalOperatorFactory(ILogicalOperators.Create());

        // act.
        Action failedCreateOperatorAction = () =>
            logicalOperatorFactory.CreateLogicalOperator(logicalOperatorName: "something else");

        //assert
        ArgumentException exception =
            Assert.Throws<ArgumentOutOfRangeException>(failedCreateOperatorAction);
    }

    [Fact]
    public void CreateFilter_WithNullFilterName_ThrowsExpectedException()
    {
        // arrange.
        ILogicalOperatorFactory logicalOperatorFactory = new LogicalOperatorFactory(ILogicalOperators.Create());

        // act.
        Action failedCreateOperatorAction = () =>
            logicalOperatorFactory.CreateLogicalOperator(logicalOperatorName: null!);

        //assert
        ArgumentException exception =
            Assert.Throws<ArgumentNullException>(failedCreateOperatorAction);

        Assert.Equal("Value cannot be null. (Parameter 'logicalOperatorName')", exception.Message);
    }

    [Fact]
    public void CreateFilter_WithEmptyFilterName_ThrowsExpectedException()
    {
        // arrange.
        ILogicalOperatorFactory logicalOperatorFactory = new LogicalOperatorFactory(ILogicalOperators.Create());
        // act.
        Action failedCreateOperatorAction = () =>
            logicalOperatorFactory.CreateLogicalOperator(logicalOperatorName: "");

        //assert
        ArgumentException exception =
            Assert.Throws<ArgumentException>(failedCreateOperatorAction);

        Assert.Equal("The value cannot be an empty string or composed entirely of whitespace. (Parameter 'logicalOperatorName')", exception.Message);
    }

    [Fact]
    public void CreateFilter_WithWhitespaceFilterName_ThrowsExpectedException()
    {
        // arrange.
        ILogicalOperatorFactory logicalOperatorFactory = new LogicalOperatorFactory(ILogicalOperators.Create());

        // act.
        Action failedCreateOperatorAction = () =>
            logicalOperatorFactory.CreateLogicalOperator(logicalOperatorName: "    ");

        //assert
        ArgumentException exception =
            Assert.Throws<ArgumentException>(failedCreateOperatorAction);

        Assert.Equal("The value cannot be an empty string or composed entirely of whitespace. (Parameter 'logicalOperatorName')", exception.Message);
    }
}
