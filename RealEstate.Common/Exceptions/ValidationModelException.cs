using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Common.Exceptions
{
    public class ValidationModelException : BaseException, ISerializable
    {
        public List<string> Messages = new List<string>();
        public ValidationModelException()
           : base()
        {
        }

        public ValidationModelException(string message)
           : base(message)
        {
            Messages.Add(message);
        }
		public ValidationModelException(List<string> messages)
		   : base(string.Join("\n",messages))
		{
			Messages = messages;
		}
		public ValidationModelException(string message, System.Exception inner)
           : base(message, inner)
        {
            Messages.Add(message);
        }

    }
}
