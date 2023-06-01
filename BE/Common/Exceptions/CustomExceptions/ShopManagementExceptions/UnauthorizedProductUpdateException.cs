using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Exceptions.CustomExceptions.ShopManagementExceptions
{
    public class UnauthorizedProductUpdateException : Exception
    {
        public UnauthorizedProductUpdateException() : base() { }
        public UnauthorizedProductUpdateException(string message) : base(message) { }
        public UnauthorizedProductUpdateException(string message, params object[] args) :
            base(string.Format(CultureInfo.CurrentCulture, message, args))
        { }
    }
}
