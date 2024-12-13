using System;
using System.Collections.Generic;

namespace Toolsfactory.Common
{
    /// <summary>
    /// Represents the result of an operation, encapsulating success or failure state and associated errors, with a value.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    public class Result<T> : Result, IResult<T>
    {
        private T? _value;

        /// <summary>
        /// Gets the value of the result. Throws an exception if the result is faulted.
        /// </summary>
        public T Value
        {
            get
            {
                if (IsSuccess) return _value!;
                throw new InvalidOperationException("Cannot access value of a faulted result");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the result is successful.
        /// </summary>
        public override bool IsSuccess { get => base.IsSuccess; protected set { base.IsSuccess = value; if (!value) _value = default; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="Result{T}"/> class with a value.
        /// </summary>
        /// <param name="value">The value of the result.</param>
        protected Result(T value) : base()
        {
            _value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Result{T}"/> class with an error.
        /// </summary>
        /// <param name="error">The error of the result.</param>
        protected Result(Error error) : base(error)
        {
            _value = default;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Result{T}"/> class with multiple errors.
        /// </summary>
        /// <param name="errors">The errors of the result.</param>
        protected Result(IEnumerable<Error> errors) : base(errors)
        {
            _value = default;
        }


        /// <summary>
        /// Implicitly converts an error to a result.
        /// </summary>
        /// <param name="error">The error to convert.</param>
        /// <returns>A result instance.</returns>
        public static implicit operator Result<T>(Error error) => new Result<T>(error);

        /// <summary>
        /// Implicitly converts a value to a result.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A result instance.</returns>
        public static implicit operator Result<T>(T value) => new Result<T>(value);

        /// <summary>
        /// Creates a successful result with a value.
        /// </summary>
        /// <param name="value">The value of the result.</param>
        /// <returns>A successful result instance.</returns>
        public static Result<T> Success(T value) => new Result<T>(value);

        /// <summary>
        /// Creates a failed result with an error.
        /// </summary>
        /// <param name="error">The error of the result.</param>
        /// <returns>A failed result instance.</returns>
        public new static Result<T> Failure(Error error) => new Result<T>(error);

        /// <summary>
        /// Creates a failed result with a default error.
        /// </summary>
        /// <returns>A failed result instance.</returns>
        public new static Result<T> Failure() => new Result<T>(Error.Default);

        /// <summary>
        /// Creates a failed Result with a message.
        /// </summary>
        /// <param name="message">The error message to include in the Result.</param>
        /// <returns>A failed Result instance.</returns>
        public new static Result<T> Failure(string message) => new Result<T>(new Error(message));
        
        /// <summary>
        /// Creates a failed Result with a list of errors.
        /// </summary>
        /// <param name="errors">The errors to include in the Result.</param>
        /// <returns>A failed Result instance.</returns>
        public new static Result<T> Failure(IEnumerable<Error> errors) => new Result<T>(errors);

    }
}