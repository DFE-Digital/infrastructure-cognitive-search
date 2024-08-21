namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.LogicalOperators.Factories;

/// <summary>
/// 
/// </summary>
public sealed class LogicalOperatorFactory : ILogicalOperatorFactory
{
    private readonly Dictionary<string, Func<ILogicalOperator>> _logicalOperatorFactory;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logicalOperatorFactory"></param>
    public LogicalOperatorFactory(Dictionary<string, Func<ILogicalOperator>> logicalOperatorFactory)
    {
        _logicalOperatorFactory = logicalOperatorFactory;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TLogicalOperator"></typeparam>
    /// <returns></returns>
    public ILogicalOperator CreateLogicalOperator<TLogicalOperator>()
        where TLogicalOperator : ILogicalOperator => CreateLogicalOperator(typeof(TLogicalOperator));

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logicalOperatorType"></param>
    /// <returns></returns>
    public ILogicalOperator CreateLogicalOperator(Type logicalOperatorType) => CreateLogicalOperator(logicalOperatorName: logicalOperatorType.Name);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logicalOperatorName"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public ILogicalOperator CreateLogicalOperator(string logicalOperatorName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(logicalOperatorName);

        return (!_logicalOperatorFactory
            .TryGetValue(logicalOperatorName, out var logicalOperator) || logicalOperator is null) ?
                throw new ArgumentOutOfRangeException(
                    $"Logical operator of type {logicalOperatorName} is not registered.") : logicalOperator();
    }
}
