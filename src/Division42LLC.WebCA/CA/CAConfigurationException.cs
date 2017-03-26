using System;
using System.Collections.Generic;
using System.Text;

namespace Division42LLC.WebCA.CA
{
    public class CAConfigurationException : CAException
    {
        public CAConfigurationException() { }
        public CAConfigurationException(string message) : base(message) { }
        public CAConfigurationException(string message, Exception inner) : base(message, inner) { }
    }
}
