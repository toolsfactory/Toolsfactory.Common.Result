using System;
using System.Collections.Generic;

namespace Toolsfactory.Common
{
    /// <summary>
    /// Represents the result of an operation, encapsulating success or failure state and associated errors.
    /// </summary>
    public interface IResult
    {
        /// <summary>
        /// Gets a value indicating whether the result is successful.
        /// </summary>
        bool IsSuccess { get; }

        /// <summary>
        /// Gets a value indicating whether the result is faulted.
        /// </summary>
        bool IsFaulted { get; }

        /// <summary>
        /// Gets the list of errors associated with the result.
        /// </summary>
        IReadOnlyList<Error> Errors { get; }
    }
}