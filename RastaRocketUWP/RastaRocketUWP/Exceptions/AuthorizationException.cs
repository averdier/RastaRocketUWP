using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RastaRocketUWP.Exceptions
{
    public class AuthorizationException : Exception
    {
        public AuthorizationException() : base() { }
        public AuthorizationException(string message) : base(message) { }
        public AuthorizationException(string message, Exception inner) : base(message, inner) { }
        protected AuthorizationException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) { }
    }
}
