using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Exceptions.CustomExceptions
{
    public class InvalidCredentialsException : Exception
    {
        public InvalidCredentialsException() : base() { }
        public InvalidCredentialsException(string message) : base(message) { }
        public InvalidCredentialsException(string message, params object[] args) :
            base(string.Format(CultureInfo.CurrentCulture, message, args))
        { }
    }
}
