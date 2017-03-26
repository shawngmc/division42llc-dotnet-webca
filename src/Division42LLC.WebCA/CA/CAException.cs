using System;
using System.Collections.Generic;
using System.Text;

namespace Division42LLC.WebCA.CA
{
    public class CAException : Exception
    {
        public CAException() { }
        public CAException(string message) : base(message) { }
        public CAException(string message, Exception inner) : base(message, inner) { }
    }
}
