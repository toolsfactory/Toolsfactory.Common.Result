using System.Collections.Generic;
using System;
using System.Runtime.CompilerServices;

namespace Toolsfactory.Common
{

    /// <summary>
    /// Represents an error with a message, code, optional exception, and metadata.
    /// </summary>
    public class Error
    {
        /// <summary>
        /// The default error instance.
        /// </summary>
        public static readonly Error Default = new ("Default", 0);

        /// <summary>
        /// Gets the error message.
        /// </summary>
        public string Message { get; init; }

        /// <summary>
        /// Gets the error code.
        /// </summary>
        public int Code { get; init; }

        /// <summary>
        /// Gets the metadata associated with the error.
        /// </summary>
        public IDictionary<string, object> Metadata { get; init; } = new Dictionary<string, object>();

        /// <summary>
        /// Gets a value indicating whether the error has an associated exception.
        /// </summary>
        public bool HasException => Exception != null;

        /// <summary>
        /// Gets the exception associated with the error, if any.
        /// </summary>
        public Exception? Exception { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Error"/> class with the specified message, code, and optional exception.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="code">The error code.</param>
        /// <param name="exception">The associated exception, if any.</param>
        public Error(string message, int code = 0, Exception? exception = null)
        {
            Message = message;
            Code = code;
            Exception = exception;
        }

        /// <summary>
        /// Adds metadata to the error.
        /// </summary>
        /// <param name="key">The metadata key.</param>
        /// <param name="value">The metadata value.</param>
        /// <returns>The current <see cref="Error"/> instance.</returns>
        public Error AddMetadata(string key, object value)
        {
            Metadata.Add(key, value);
            return this;
        }

        /// <summary>
        /// Creates an <see cref="Error"/> instance from an exception.
        /// </summary>
        /// <param name="exception">The exception to create the error from.</param>
        /// <returns>An <see cref="Error"/> instance representing the exception.</returns>
        public static Error FromException(Exception exception)
        {
            return new Error(exception.Message, exception.HResult, exception);
        }

        public static Error FromException(Exception exception, string message, int code = int.MinValue)
        {
            return new Error(message, code == int.MinValue ? exception.HResult : code, exception);
        }

        public static Error FromException(Exception exception, int code)
        {
            return new Error(exception.Message, code, exception);
        }
    }
}