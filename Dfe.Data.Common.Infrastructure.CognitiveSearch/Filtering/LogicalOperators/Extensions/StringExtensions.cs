﻿namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.LogicalOperators.Extensions;

/// <summary>
/// Provides an extension to string padding by allowing padding
/// to be applied to both sides and of equally prescribed width.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Allows padding to be provisioned equally to both sides of the provisioned
    /// string, the width will default to one if no value is assigned.
    /// </summary>
    /// <param name="value">
    /// The string on which to apply the padding operation.
    /// </param>
    /// <param name="paddingWidth">
    /// The size of the padding width to apply (evenly) to both sides of the string.
    /// </param>
    /// <returns>
    /// The original string value formatted with the padding width applied evenly to both sides.
    /// </returns>
    public static string PadSides(this string value, int paddingWidth = 1)
    {
        int leftPaddingWidth = value.Length + paddingWidth;
        int rightPaddingWidth = leftPaddingWidth + paddingWidth;

        return value.PadLeft(leftPaddingWidth).PadRight(rightPaddingWidth);
    }
}