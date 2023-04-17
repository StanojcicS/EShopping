using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Exceptions.CustomExceptions
{
    public class UserAlreadyExistsException : Exception
    {
        public UserAlreadyExistsException() : base() { }
        public UserAlreadyExistsException(string message) : base(message) { }
        public UserAlreadyExistsException(string message, params object[] args) :
            base(string.Format(CultureInfo.CurrentCulture, message, args))
        { }

    }
}
