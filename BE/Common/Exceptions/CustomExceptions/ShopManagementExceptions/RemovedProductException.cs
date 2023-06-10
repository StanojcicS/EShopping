using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Exceptions.CustomExceptions.ShopManagementExceptions
{
    public class RemovedProductException : Exception
    {
        public RemovedProductException() : base() { }
        public RemovedProductException(string message) : base(message) { }
        public RemovedProductException(string message, params object[] args) :
            base(string.Format(CultureInfo.CurrentCulture, message, args))
        { }
    }
}
