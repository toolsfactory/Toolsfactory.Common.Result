using System;
using System.Collections.Generic;
using System.Linq;

namespace Toolsfactory.Common
{
    /// <summary>
    /// Represents the result of an operation, encapsulating success or failure state and associated errors.
    /// </summary>
    public class Result : IResult
    {
        private const string InvalidErrorAccessMessage = "Cannot access RootError of a successful result";

        protected readonly List<Error> _errors = new();

        /// <summary>
        /// Gets or sets a value indicating whether the result is successful.
        /// </summary>
        public virtual bool IsSuccess { get; protected set; }

        /// <summary>
        /// Gets a value indicating whether the result is faulted.
        /// </summary>
        public bool IsFaulted => !IsSuccess;

        /// <summary>
        /// The root error of the result. If the result is successful, throws an <see cref="InvalidOperationException"/>.
        /// </summary>
        public Error RootError => IsFaulted ? _errors.FirstOrDefault() ?? Error.Default : throw new InvalidOperationException(InvalidErrorAccessMessage);

        /// <summary>
        /// Gets the list of errors associated with the result.
        /// </summary>
        public IReadOnlyList<Error> Errors => _errors.AsReadOnly();

        protected Result(bool isSuccess, IEnumerable<Error>? errors)
        {
            IsSuccess = isSuccess;
            if (IsFaulted && errors != null)
            {
                _errors.AddRange(errors);
            }
        }

        /// <summary>
        /// Adds an error to the current result and sets the IsSuccess flag to false.
        /// </summary>
        /// <param name="error">The error to add</param>
        /// <returns>The result</returns>
        public virtual Result AddError(Error error)
        {
            IsSuccess = false;
            _errors.Add(error);
            return this;
        }

        /// <summary>
        /// Adds a list of errors to the current result and sets the IsSuccess flag to false.
        /// </summary>
        /// <param name="errors">The errors to add</param>
        /// <returns>The result</returns>
        public virtual Result AddErrors(IEnumerable<Error> errors)
        {
            IsSuccess = false;
            _errors.AddRange(errors);
            return this;
        }

        /// <summary>
        /// Adds the errors from the provided results to the current result and sets the IsSuccess flag to false if any of the provided results are faulted.
        /// </summary>
        /// <param name="results">The other results to check and add</param>
        public void Combine(params Result[] results)
        {
            foreach (var res in results)
            {
                if (res.IsFaulted)
                {
                    IsSuccess = false;
                    _errors.AddRange(res.Errors);
                }
            }
        }

        /// <summary>
        /// Implicitly converts a boolean to a Result. Returns Success if true, otherwise Failure.
        /// </summary>
        /// <param name="success">The boolean value indicating success or failure.</param>
        /// <returns>A Result instance.</returns>
        public static implicit operator Result(bool success) => success ? Success() : Failure();

        /// <summary>
        /// Implicitly converts an Error to a failure Result.
        /// </summary>
        /// <param name="error">The error to convert.</param>
        /// <returns>A Result instance.</returns>
        public static implicit operator Result(Error error) => Failure(error);

        /// <summary>
        /// Implicitly converts a <see cref="List{Error}"/> to a failure Result.
        /// </summary>
        /// <param name="error">The error to convert.</param>
        /// <returns>A Result instance.</returns>
        public static implicit operator Result(List<Error> errors) => Failure(errors);

        /// <summary>
        /// Implicitly converts an <see cref="Exception"> to a Result indicating a failure.
        /// </summary>
        /// <param name="exception">The exception to convert.</param>
        /// <returns>A Result instance.</returns>
        public static implicit operator Result(Exception ex) => Failure(Error.FromException(ex));

        /// <summary>
        /// Creates a successful Result.
        /// </summary>
        /// <returns>A successful Result instance.</returns>
        public static Result Success() => new Result(true, null);

        /// <summary>
        /// Creates a failed Result with a list of errors.
        /// </summary>
        /// <param name="errors">The errors to include in the Result.</param>
        /// <returns>A failed Result instance.</returns>
        public static Result Failure(IEnumerable<Error> errors) => new Result(false, errors);

        /// <summary>
        /// Creates a failed Result with a single error.
        /// </summary>
        /// <param name="error">The error to include in the Result.</param>
        /// <returns>A failed Result instance.</returns>
        public static Result Failure(Error error) => new Result(false, [error]);

        /// <summary>
        /// Creates a failed Result with a default error.
        /// </summary>
        /// <returns>A failed Result instance.</returns>
        public static Result Failure() => new Result(false, [Error.Default]);

        /// <summary>
        /// Creates a failed Result with a message.
        /// </summary>
        /// <param name="message">The error message to include in the Result.</param>
        /// <returns>A failed Result instance.</returns>
        public static Result Failure(string message) => new Result(false, [new Error(message)]);
    }
}