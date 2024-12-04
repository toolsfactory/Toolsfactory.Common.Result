using System;
using System.Collections.Generic;

namespace Toolsfactory.Common
{
    public static class ResultExtensions
    {
        /// <summary>
        /// Handles the result by invoking the appropriate action based on the success or failure state.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="result">The result to handle.</param>
        /// <param name="onSuccess">The action to invoke if the result is successful.</param>
        /// <param name="onFailure">The action to invoke if the result is faulted.</param>
        public static void Switch<T>(this Result<T> result,
                                        Action<T> onSuccess,
                                        Action<IReadOnlyList<Error>> onFailure)
        {
            if (result.IsSuccess)
            {
                onSuccess(result.Value);
            }
            else
            {
                onFailure(result.Errors);
            }
        }

        /// <summary>
        /// Handles the result by invoking the appropriate action based on the success or failure state.
        /// </summary>
        /// <param name="result">The result to handle.</param>
        /// <param name="onSuccess">The action to invoke if the result is successful.</param>
        /// <param name="onFailure">The action to invoke if the result is faulted.</param>
        public static void Switch(this Result result,
                                    Action onSuccess,
                                    Action<IReadOnlyList<Error>> onFailure)
        {
            if (result.IsSuccess)
            {
                onSuccess();
            }
            else
            {
                onFailure(result.Errors);
            }
        }

        /// <summary>
        /// Maps the success or failure state of the result to a new type.
        /// </summary>
        /// <typeparam name="T">The type of the return value.</typeparam>
        /// <param name="result">The result to switch on.</param>
        /// <param name="onSuccess">The function to invoke if the result is successful.</param>
        /// <param name="onFailure">The function to invoke if the result is faulted.</param>
        /// <returns>The result of the invoked function.</returns>
        public static T Map<T>(this Result result,
                                    Func<T> onSuccess,
                                    Func<IReadOnlyList<Error>, T> onFailure)
        {
            return result.IsSuccess ? onSuccess() : onFailure(result.Errors);
        }

        /// <summary>
        /// Maps the success or failure state of the result to a new type.
        /// </summary>
        /// <typeparam name="X">The type of the return value.</typeparam>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="result">The result to base the mapping on.</param>
        /// <param name="onSuccess">The function to invoke if the result is successful.</param>
        /// <param name="onFailure">The function to invoke if the result is faulted.</param>
        /// <returns>The result of the invoked function.</returns>
        public static X Map<X, T>(this Result<T> result,
                                    Func<T, X> onSuccess,
                                    Func<IReadOnlyList<Error>, X> onFailure)
        {
            return result.IsSuccess ? onSuccess(result.Value) : onFailure(result.Errors);
        }
    }
}