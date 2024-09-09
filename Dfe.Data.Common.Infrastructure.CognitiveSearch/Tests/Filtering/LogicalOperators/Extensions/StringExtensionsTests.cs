using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.LogicalOperators.Extensions;
using FluentAssertions;
using Xunit;

namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.Tests.Filtering.LogicalOperators.Extensions;

public sealed class StringExtensionsTests
{
    [Fact]
    public void PadSides_WithValidString_AddsDefaultSinglePadSpaceEquallyToBothSides()
    {
        // arrange
        const string TestString = "I_expect_to_be_padded_with_whitespace_equally_both_sides";
        const string ExpectedResult = " I_expect_to_be_padded_with_whitespace_equally_both_sides ";
        
        // act
        string result = TestString.PadSides();

        // assert
        result.Should().NotBeNull().And.Be(ExpectedResult);
    }

    [Fact]
    public void PadSides_WithValidString_AddsCustomSpecifiedPadSpaceEquallyToBothSides()
    {
        // arrange
        const string TestString = "I_expect_to_be_padded_with_whitespace_equally_both_sides";
        const string ExpectedResult = "   I_expect_to_be_padded_with_whitespace_equally_both_sides   ";
        const int PadLength = 3;

        // act
        string result = TestString.PadSides(PadLength);

        // assert
        result.Should().NotBeNull().And.Be(ExpectedResult);
    }

    [Fact]
    public void PadSides_WithNullString_ThrowsExpectedException()
    {
        // arrange
        const string TestString = null!;

        // act/assert
        Action failedPadSidesAction = () => TestString!.PadSides();

        //assert
        ArgumentNullException exception =
            Assert.Throws<ArgumentNullException>(failedPadSidesAction);

        Assert.Equal("Value cannot be null. (Parameter 'value')", exception.Message);
    }

    [Fact]
    public void PadSides_WithEmptyString_ThrowsExpectedException()
    {
        // arrange
        const string TestString = "";

        // act/assert
        Action failedPadSidesAction = () => TestString!.PadSides();

        //assert
        ArgumentException exception =
            Assert.Throws<ArgumentException>(failedPadSidesAction);

        Assert.Equal("The value cannot be an empty string or composed entirely of whitespace. (Parameter 'value')", exception.Message);
    }

    [Fact]
    public void PadSides_WithWhitespaceString_ThrowsExpectedException()
    {
        // arrange
        const string TestString = "       ";

        // act/assert
        Action failedPadSidesAction = () => TestString!.PadSides();

        //assert
        ArgumentException exception =
            Assert.Throws<ArgumentException>(failedPadSidesAction);

        Assert.Equal("The value cannot be an empty string or composed entirely of whitespace. (Parameter 'value')", exception.Message);
    }

    [Fact]
    public void PadSides_WithZeroPadLength_ThrowsExpectedException()
    {
        // arrange
        const string TestString = "I_expect_to_be_padded_with_whitespace_equally_both_sides";
        const int PadLength = 0;

        // act/assert
        Action failedPadSidesAction = () => TestString.PadSides(PadLength);

        ArgumentException exception =
           Assert.Throws<ArgumentException>(failedPadSidesAction);

        Assert.Equal("Please specify a positive padding number. (Parameter 'paddingWidth')", exception.Message);
    }

    [Fact]
    public void PadSides_WithNegativePadLength_ThrowsExpectedException()
    {
        // arrange
        const string TestString = "I_expect_to_be_padded_with_whitespace_equally_both_sides";
        const int PadLength = -1;

        // act/assert
        Action failedPadSidesAction = () => TestString.PadSides(PadLength);

        ArgumentException exception =
           Assert.Throws<ArgumentException>(failedPadSidesAction);

        Assert.Equal("Please specify a positive padding number. (Parameter 'paddingWidth')", exception.Message);
    }
}
