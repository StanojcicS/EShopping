using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Exceptions.CustomExceptions
{
    public class InvalidUsernameException : Exception
    {
        public InvalidUsernameException() : base() { }
        public InvalidUsernameException(string message) : base(message) { }
        public InvalidUsernameException(string message, params object[] args) :
            base(string.Format(CultureInfo.CurrentCulture, message, args))
        { }

    }
}
