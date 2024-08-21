using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.LogicalOperators.Extensions;

namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.LogicalOperators;

/// <summary>
/// 
/// </summary>
public sealed class AndLogicalOperator : ILogicalOperator
{
    /// <summary>
    /// 
    /// </summary>
    const string LogicOperator = "and";

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public string CreateLogicalOperator() => LogicOperator.PadSides();
}