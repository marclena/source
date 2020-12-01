using System;
using System.Collections.Generic;

namespace XX.Template.Core.Extensions
{
    public static class OperationResultExtensions
    {
        public static OperationResult<TResult> AddResult<TResult>(
            this OperationResult<TResult> operationResult,
            TResult result)
        {
            operationResult.Result = result;
            return operationResult;
        }

        public static OperationResult<TResult> AddResultWithError<TResult>(
            this OperationResult<TResult> operationResult,
            TResult result,
            string errorMessage,
            int errorCode,
            Exception exception = null)
        {
            operationResult.Result = result;
            operationResult.Errors.Add(new ErrorResult(errorMessage, errorCode, exception));
            return operationResult;
        }

        public static OperationResult<TResult> AddResultWithError<TResult>(
            this OperationResult<TResult> operationResult,
            TResult result,
            ErrorResult errorResult)
        {
            operationResult.Result = result;
            operationResult.Errors.Add(errorResult);
            return operationResult;
        }

        public static OperationResult<TResult> AddError<TResult>(
            this OperationResult<TResult> operationResult,
            string errorMessage,
            int errorCode,
            Exception exception = null)
        {
            operationResult.Errors.Add(new ErrorResult(errorMessage, errorCode, exception));
            return operationResult;
        }

        public static OperationResult<TResult> AddError<TResult>(
            this OperationResult<TResult> operationResult,
            ErrorResult errorResult)
        {
            operationResult.Errors.Add(errorResult);
            return operationResult;
        }

        public static OperationResult<TResult> AddErrors<TResult>(
            this OperationResult<TResult> operationResult,
            IEnumerable<ErrorResult> validationErrors)
        {
            operationResult.Errors.AddRange(validationErrors);
            return operationResult;
        }

    }
}