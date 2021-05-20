using Proteo5.HL.Validators;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proteo5.HL
{
    public class Result
    {
        public Result()
        {
            this.State = ResultsStates.success;
            this.ValidationResults = new ValidatorResults();
        }

        public Result(string state)
        {
            this.State = state;
            this.ValidationResults = new ValidatorResults();
        }

        public string State { get; set; }
        public string Message { get; set; }
        public ValidatorResults ValidationResults { get; set; }
        public Exception Exception { get; set; }

        public Result<object> GetResult
        {
            get
            {
                return new()
                {
                    State = this.State,
                    Message = this.Message,
                    ValidationResults = this.ValidationResults,
                    Exception = this.Exception,
                    Data = null
                };
            }
        }
    }

    public class Result<T> : Result
    {
        public Result() : base()
        {
            this.State = ResultsStates.success;
            this.ValidationResults = new ValidatorResults();
        }

        public Result(string state) : base(state)
        {
            this.State = state;
            this.ValidationResults = new ValidatorResults();
        }

        public T Data { get; set; }

        public new Result GetResult
        {
            get
            {
                return new()
                {
                    State = this.State,
                    Message = this.Message,
                    ValidationResults = this.ValidationResults,
                    Exception = this.Exception
                };
            }
        }
    }

    public class ResultsStates
    {
        public const string success = "success";
        public const string unsuccess = "unsuccess";
        public const string empty = "empty";
        public const string invalid = "invalid";
        public const string error = "error";
    }

}
