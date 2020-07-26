using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Common.Exceptions
{
	public class AuthenticationException : BaseException, ISerializable
	{
		public AuthenticationException()
           : base()
        {
		}

		public AuthenticationException(string message)
           : base(message)
        {
		}

		public AuthenticationException(string message, System.Exception inner)
           : base(message, inner)
        {
		}

		protected AuthenticationException(SerializationInfo info, StreamingContext context)
           : base(info, context)
        {
		}

	}
}
