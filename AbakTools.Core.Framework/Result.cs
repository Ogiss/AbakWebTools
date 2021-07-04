using System;

namespace AbakTools.Core.Framework
{
    public class Result
    {
        public string FailMessage { get; private set; }
        public bool IsSuccess { get; private set; }
        public bool IsFail => !IsSuccess;

        protected Result() { }

        protected Result(bool isSuccess, string failMessage)
        {
            IsSuccess = isSuccess;
            FailMessage = failMessage;
        }

        public Result OnSuccess(Action<Result> onSuccess)
        {
            if (IsSuccess && onSuccess != null)
            {
                onSuccess(this);
            }

            return this;
        }

        public Result OnSuccess(Action onSuccess)
        {
            if (IsSuccess && onSuccess != null)
            {
                onSuccess();
            }

            return this;
        }

        public Result OnFail(Action<Result> onFail)
        {
            if (IsFail && onFail != null)
            {
                onFail(this);
            }

            return this;
        }

        public Result OnFail(Action onFail)
        {
            if (IsFail && onFail != null)
            {
                onFail();
            }

            return this;
        }

        public Result OnComplete(Action onComplete)
        {
            if (onComplete != null)
            {
                onComplete();
            }

            return this;
        }

        public Result OnComplete(Action<Result> onComplete)
        {
            if (onComplete != null)
            {
                onComplete(this);
            }

            return this;
        }

        public static Result Success()
        {
            return new Result(true, null);
        }

        public static Result Fail(string message = null)
        {
            return new Result(false, message);
        }

        public static Result Of(bool condition, string failMessage = null)
        {
            return condition ? Success() : Fail(failMessage);
        }
    }

    public class Result<TResult> : Result
    {
        public TResult SuccesResult { get; private set; }

        private Result(bool isSuccess, string failMessage, TResult result)
            : base(isSuccess, failMessage)
        {
            SuccesResult = result;
        }

        public Result<TResult> OnSuccess(Action<Result<TResult>> onSuccess)
        {
            if (IsSuccess && onSuccess != null)
            {
                onSuccess(this);
            }

            return this;
        }

        public Result<TResult> OnFail(Action<Result<TResult>> onFail)
        {
            if (IsFail && onFail != null)
            {
                onFail(null);
            }

            return this;
        }

        public Result<TResult> OnComplete(Action<Result<TResult>> onComplete)
        {
            if (onComplete != null)
            {
                onComplete(this);
            }

            return this;
        }

        public static Result<TResult> Success(TResult result)
        {
            return new Result<TResult>(true, null, result);
        }

        public static new Result<TResult> Fail(string message = null)
        {
            return new Result<TResult>(false, message, default);
        }

        public static Result<TResult> IsNotNull(TResult result, string failMessage)
        {
            return result != null ? Success(result) : Fail(failMessage);
        }
    }
}
