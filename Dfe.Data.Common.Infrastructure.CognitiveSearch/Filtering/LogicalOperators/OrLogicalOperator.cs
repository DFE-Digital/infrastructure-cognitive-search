using Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.LogicalOperators.Extensions;

namespace Dfe.Data.Common.Infrastructure.CognitiveSearch.Filtering.LogicalOperators
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class OrLogicalOperator : ILogicalOperator
    {
        /// <summary>
        /// 
        /// </summary>
        const string LogicOperator = "or";

        /// <summary>s
        /// 
        /// </summary>
        /// <returns></returns>
        public string CreateLogicalOperator() => LogicOperator.PadSides();
    }
}
