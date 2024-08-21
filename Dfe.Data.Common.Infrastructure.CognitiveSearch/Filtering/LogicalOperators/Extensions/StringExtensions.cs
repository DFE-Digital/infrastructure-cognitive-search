namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.LogicalOperators.Extensions;

/// <summary>
/// 
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="paddingWidth"></param>
    /// <returns></returns>
    public static string PadSides(this string value, int paddingWidth = 1)
    {
        int leftPaddingWidth = value.Length + paddingWidth;
        int rightPaddingWidth = leftPaddingWidth + paddingWidth;

        return value.PadLeft(leftPaddingWidth).PadRight(rightPaddingWidth);
    }
}