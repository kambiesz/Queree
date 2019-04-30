using System;
using System.Collections.Generic;
using System.Text;

namespace Queree.Commands
{
    [Serializable]
    public class InvalidCommandException : Exception
    {
        public string[] Errors { get; }

        public InvalidCommandException(string message, string[] errors) : base(message)
        {
            Errors = errors;
        }
    }
}
