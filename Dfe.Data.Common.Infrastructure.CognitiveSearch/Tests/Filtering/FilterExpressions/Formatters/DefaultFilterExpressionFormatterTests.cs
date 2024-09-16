using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.FilterExpressions.Formatters;
using Xunit;

namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.Tests.Filtering.FilterExpressions.Formatters;

public sealed class DefaultFilterExpressionFormatterTests
{
    [Theory]
    [InlineData(new object[] { "one" }, "'{0}'", "'one'")]
    [InlineData(new object[] { "one", "two" }, "'{0},{1}'", "'one,two'")]
    [InlineData(new object[] { "one", "two", "three" }, "'{0},{1}'", "'one,two'")]
    public void CreateFormattedExpression_ReturnsFormattedExpression(object[] parameters, string parameteriseString, string expectedResult)
    {
        // arrange
        var formatter = new DefaultFilterExpressionFormatter();

        // act
        var result = formatter.CreateFormattedExpression(parameteriseString, parameters);

        // assert
        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData(new object[] { "one" }, "", typeof(ArgumentException))]
    [InlineData(new object[] { }, "'{0},{1}'", typeof(ArgumentException))]
    [InlineData(new object[] { "one" }, "'{0},{1}'", typeof(FormatException))]
    public void CreateFormattedExpression_Invalidparameters_ThrowsException(object[] parameters, string parameteriseString, Type exceptionType)
    {
        // arrange
        var formatter = new DefaultFilterExpressionFormatter();

        // act
        var exception =
            Assert.ThrowsAny<Exception>(() =>
                formatter.CreateFormattedExpression(parameteriseString, parameters));

        // assert
        Assert.Equal(exceptionType, exception.GetType());
    }

    [Theory]
    [InlineData(new object[] { "one" }, "{0}")]
    [InlineData(new object[] { "one", "two" }, "{0},{1}")]
    [InlineData(new object[] { 1 }, "{0}")]
    [InlineData(new object[] { }, "")]
    public void CreateFilterCriteriaPlaceholders_WithExpressionParamsSeparatorSet_ReturnsSeparatedParameteriseString(object[] input, string expectedResult)
    {
        // arrange
        var formatter = new DefaultFilterExpressionFormatter();
        formatter.SetExpressionParamsSeparator(",");

        // act
        var result = formatter.CreateFilterCriteriaPlaceholders(input);

        // assert
        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData(new object[] { "one" }, "{0}")]
    [InlineData(new object[] { "one", "two" }, "{0}{1}")]
    [InlineData(new object[] { 1 }, "{0}")]
    [InlineData(new object[] { }, "")]
    
    public void CreateFilterCriteriaPlaceholders_WithoutExpressionParamsSeparatorSet_ReturnsUnseparatedParameteriseString(object[] input, string expectedResult)
    {
        // arrange
        var formatter = new DefaultFilterExpressionFormatter();

        // act
        var result = formatter.CreateFilterCriteriaPlaceholders(input);

        // assert
        Assert.Equal(expectedResult, result);
    }
}