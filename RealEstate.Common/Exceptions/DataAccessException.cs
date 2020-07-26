using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Common.Exceptions
{
    public class DataAccessException : BaseException, ISerializable
    {
        public DataAccessException()
           : base()
        {
        }

        public DataAccessException(string message)
           : base(message)
        {
        }

        public DataAccessException(string message, System.Exception inner)
           : base(message, inner)
        {
        }

        protected DataAccessException(SerializationInfo info, StreamingContext context)
           : base(info, context)
        {
        }

    }
}
