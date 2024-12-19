using System;

namespace Toolsfactory.Common
{
    /// <summary>
    /// Represents the result of an operation, encapsulating success or failure state and associated value.
    /// </summary>
    public interface IResult<T> : IResult
    {
        /// <summary>
        /// Gets the value of the result.
        /// </summary>
        T Value { get; }
    }
}