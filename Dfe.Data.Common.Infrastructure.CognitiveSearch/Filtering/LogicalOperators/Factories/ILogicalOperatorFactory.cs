namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.LogicalOperators.Factories;

/// <summary>
/// Provides an abstraction over which to derive Azure AI OData logical
/// operators through a configured factory implementation which supports
/// creation of <see cref="ILogicalOperator"/> objects based on type name, type, or generic type.
/// </summary>
public interface ILogicalOperatorFactory
{
    /// <summary>
    /// Allows creation of an <see cref="ILogicalOperator"/> instance based on the type requested.
    /// </summary>
    /// <param name="logicalOperatorType">
    /// The concrete implementation type of <see cref="ILogicalOperator"/> requested.
    /// </param>
    /// <returns>
    /// The configured instance of the <see cref="ILogicalOperator"/> type.
    /// </returns>
    ILogicalOperator CreateLogicalOperator(Type logicalOperatorType);

    /// <summary>
    /// Allows creation of an <see cref="ILogicalOperator"/> instance based on the type name requested.
    /// </summary>
    /// <param name="logicalOperatorName">
    /// The name of the concrete implementation type <see cref="ILogicalOperator"/> requested.
    /// </param>
    /// <returns>
    /// The configured instance of the <see cref="ILogicalOperator"/> type.
    /// </returns>
    ILogicalOperator CreateLogicalOperator(string logicalOperatorName);

    /// <summary>
    /// Allows creation of an <see cref="ILogicalOperator"/> instance based on the generic type specified.
    /// </summary>
    /// <typeparam name="TLogicalOperator">
    /// The concrete type of <see cref="ILogicalOperator"/> requested.
    /// </typeparam>
    /// <returns>
    /// The configured instance of the <see cref="ILogicalOperator"/> type.
    /// </returns>
    ILogicalOperator CreateLogicalOperator<TLogicalOperator>() where TLogicalOperator : ILogicalOperator;
}
