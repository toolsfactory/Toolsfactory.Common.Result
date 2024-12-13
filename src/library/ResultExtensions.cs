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
        /// <typeparam name="TIn">The type of the value.</typeparam>
        /// <typeparam name="TOut">The type of the return value.</typeparam>
        /// <param name="result">The result to base the mapping on.</param>
        /// <param name="onSuccess">The function to invoke if the result is successful.</param>
        /// <param name="onFailure">The function to invoke if the result is faulted.</param>
        /// <returns>The result of the invoked function.</returns>
        public static TOut Map<TIn, TOut>(this Result<TIn> result,
                                    Func<TIn, TOut> onSuccess,
                                    Func<IReadOnlyList<Error>, TOut> onFailure)
        {
            return result.IsSuccess ? onSuccess(result.Value) : onFailure(result.Errors);
        }

        /// <summary>
        /// Binds the result to a new result based on the success or failure state.
        /// </summary>
        /// <typeparam name="TIn">The type of the value.</typeparam>
        /// <typeparam name="TOut">The type of the return value.</typeparam>
        /// <param name="result">The result to base the mapping on.</param>
        /// <param name="bind">The method called in case the input result state was success</param>
        /// <returns>a new result</returns>
        public static Result<TOut> Bind<TIn, TOut>(this Result<TIn> result, Func<TIn, Result<TOut>> bind)
        {
            return result.IsSuccess ? bind(result.Value) : Result<TOut>.Failure(result.Errors);
        }

        /// <summary>
        /// Binds the result to a new result based on the state of the initial result object. This bind variant covers calls to methods that make use of classic exceptions for flow control.
        /// </summary>
        /// <typeparam name="TIn">The type of the value.</typeparam>
        /// <typeparam name="TOut">The type of the return value.</typeparam>
        /// <param name="result">The result to base the mapping on.</param>
        /// <param name="bind">The method called in case the input result state was success. The method is called in a try catch block and in case an exception occurs, an error is returned</param>
        /// <param name="error">The error returned in case an exception was catched</param>
        /// <returns>a new result</returns>
        public static Result<TOut> BindTryCatch<TIn, TOut>(this Result<TIn> result, Func<TIn, TOut> bind, Error error)
        {
            try
            {
                return result.IsSuccess ? 
                    Result<TOut>.Success(bind(result.Value)) : 
                    Result<TOut>.Failure(result.Errors);
            }
            catch (Exception ex)
            {
                return Result<TOut>.Failure(error.AddMetadata("Exception", ex));
            }
        }

        /// <summary>
        /// Binds the result to a new result based on the state of the initial result object. This bind variant covers calls to methods that make use of classic exceptions for flow control.
        /// </summary>
        /// <typeparam name="TIn">The type of the value.</typeparam>
        /// <typeparam name="TOut">The type of the return value.</typeparam>
        /// <typeparam name="TException">The type of the exception to catch</typeparam>
        /// <param name="result">The result to base the mapping on.</param>
        /// <param name="bind">The method called in case the input result state was success. The method is called in a try catch block and in case an exception occurs, an error is returned</param>
        /// <param name="error">The error returned in case an exception was catched</param>
        /// <returns>a new result</returns>        
        public static Result<TOut> BindTryCatch<TIn, TOut, TException>(this Result<TIn> result, Func<TIn, TOut> bind, Error error)  where TException : Exception
        {
            try
            {
                return result.IsSuccess ?
                    Result<TOut>.Success(bind(result.Value)) :
                    Result<TOut>.Failure(result.Errors);
            }
            catch (TException ex)
            {
                return Result<TOut>.Failure(error.AddMetadata("Exception", ex));
            }
        }

        /// <summary>
        /// Executes an action in case the result passed in was in success state but doesn't change the result itself.
        /// </summary>
        /// <typeparam name="T">The type of the return value.</typeparam>
        /// <param name="result">The result object to act on</param>
        /// <param name="action">The action to trigger in success state</param>
        /// <returns></returns>
        public static Result<T> Tap<T>(this Result<T> result, Action<T> action)
        {
            if (result.IsSuccess)
            {
                action(result.Value);
            }
            return result;
        }
    }
}