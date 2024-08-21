namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.LogicalOperators.Factories;

/// <summary>
/// 
/// </summary>
public interface ILogicalOperatorFactory
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="logicalOperatorType"></param>
    /// <returns></returns>
    ILogicalOperator CreateLogicalOperator(Type logicalOperatorType);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logicalOperatorName"></param>
    /// <returns></returns>
    ILogicalOperator CreateLogicalOperator(string logicalOperatorName);

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TLogicalOperator"></typeparam>
    /// <returns></returns>
    ILogicalOperator CreateLogicalOperator<TLogicalOperator>() where TLogicalOperator : ILogicalOperator;
}
