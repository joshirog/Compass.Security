using System;
using System.Collections.Generic;

namespace Compass.Security.Domain.Exceptions
{
    public class ErrorInvalidException : Exception
    {
        private IEnumerable<string> Errors { get; }
        
        public ErrorInvalidException(IEnumerable<string> errors)
        {
            Errors = errors;
        }
    }
}