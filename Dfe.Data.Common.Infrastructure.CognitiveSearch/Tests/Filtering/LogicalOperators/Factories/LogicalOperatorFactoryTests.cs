using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.LogicalOperators;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.LogicalOperators.Factories;
using Dfe.Data.Common.Infrastructure.CognitiveSearch.Tests.Filtering.FilterExpressions;
using FluentAssertions;
using Xunit;

namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.Tests.Filtering.LogicalOperators.Factories;

public sealed class LogicalOperatorFactoryTests
{
    [Fact]
    public void CreateLogicalOperator_WithValidOperatorName_ReturnsExpectedConfiguredOperatorFromfactory()
    {
        // arrange.
        ILogicalOperatorFactory logicalOperatorFactory =
            LogicalOperatorFactoryTestDouble.MockLogicalOperatorFactory();

        // act.
        ILogicalOperator logicalOperator =
            logicalOperatorFactory.CreateLogicalOperator("AndLogicalOperator");

        // assert.
        logicalOperator.Should().NotBeNull();
        logicalOperator.Should().BeOfType<AndLogicalOperator>();
    }

    [Fact]
    public void CreateLogicalOperator_WithValidOperatorType_ReturnsExpectedConfiguredOperatorFromfactory()
    {
        // arrange.
        ILogicalOperatorFactory logicalOperatorFactory =
            LogicalOperatorFactoryTestDouble.MockLogicalOperatorFactory();

        // act.
        ILogicalOperator logicalOperator =
            logicalOperatorFactory.CreateLogicalOperator(typeof(AndLogicalOperator));

        // assert.
        logicalOperator.Should().NotBeNull();
        logicalOperator.Should().BeOfType<AndLogicalOperator>();
    }

    [Fact]
    public void CreateLogicalOperator_WithValidOperatorGenricTemplateType_ReturnsExpectedConfiguredOperatorFromfactory()
    {
        // arrange.
        ILogicalOperatorFactory logicalOperatorFactory =
            LogicalOperatorFactoryTestDouble.MockLogicalOperatorFactoryFor<AndLogicalOperator>();

        // act.
        ILogicalOperator logicalOperator =
            logicalOperatorFactory.CreateLogicalOperator<AndLogicalOperator>();

        // assert.
        logicalOperator.Should().NotBeNull();
        logicalOperator.Should().BeOfType<AndLogicalOperator>();
    }

    [Fact]
    public void CreateFilter_WithNullFilterName_ThrowsExpectedException()
    {
        // arrange.
        ILogicalOperatorFactory logicalOperatorFactory =
           LogicalOperatorFactoryTestDouble.MockLogicalOperatorFactory();

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
        ILogicalOperatorFactory logicalOperatorFactory =
            LogicalOperatorFactoryTestDouble.MockLogicalOperatorFactory();

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
        ILogicalOperatorFactory logicalOperatorFactory =
            LogicalOperatorFactoryTestDouble.MockLogicalOperatorFactory();

        // act.
        Action failedCreateOperatorAction = () =>
            logicalOperatorFactory.CreateLogicalOperator(logicalOperatorName: "    ");

        //assert
        ArgumentException exception =
            Assert.Throws<ArgumentException>(failedCreateOperatorAction);

        Assert.Equal("The value cannot be an empty string or composed entirely of whitespace. (Parameter 'logicalOperatorName')", exception.Message);
    }
}
