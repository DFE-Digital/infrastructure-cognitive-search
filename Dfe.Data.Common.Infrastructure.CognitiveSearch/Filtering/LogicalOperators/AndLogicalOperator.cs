﻿using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.LogicalOperators.Extensions;

namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.LogicalOperators;

/// <summary>
/// Creates an OData logical operator filter expression which are Boolean expressions that evaluate to true or false.
/// This allows us to compose more complex filter expressions by writing a series of simpler filters and composing them
/// using the logical operators from Boolean algebra. The <b>and</b> binary operator evaluates to true if both its left
/// and right sub-expressions evaluate to true. For further information please visit the following link,
/// <a href="https://learn.microsoft.com/en-us/azure/search/search-query-odata-logical-operators"> OData logical operators</a>.
/// </summary>
public sealed class AndLogicalOperator : ILogicalOperator
{
    /// <summary>
    /// Constant string used to compose the <b>and</b> logical operator.
    /// </summary>
    const string LogicOperator = "and";

    /// <summary>
    /// Returns the configured <b>and</b> logical operator by applying default padding to
    /// both sides of the operator. This allows the operator to be easily nested between filter expressions.
    /// </summary>
    /// <returns>
    /// A string representing the configured <b>and</b> logical operator.
    /// </returns>
    public string GetOperatorExpression() => LogicOperator.PadSides();
}