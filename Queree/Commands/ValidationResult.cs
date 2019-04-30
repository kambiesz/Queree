using System;
using System.Collections.Generic;
using System.Text;

namespace Queree.Commands
{
    public class ValidationResult
    {
        public bool IsValid { get; }
        public string[] Errors { get;  }

        private ValidationResult(bool success, string[] errors)
        {
            this.IsValid = success;
            this.Errors = errors is null ? new string[0] : errors;
        }

        public static ValidationResult Success()
        {
            return new ValidationResult(true, new string[0]);
        }

        public static ValidationResult Error(params string[] errors)
        {
            return new ValidationResult(false, errors);
        }
    }
}
