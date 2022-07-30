using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Multigroup.DomainModel.CustomException
{
    public class EntityException : Exception
    {
        public EntityException () : base ()
        {}
       
        public EntityException (string message) : base (message)
        {}
       
        public EntityException (string message, Exception innerException)
            : base (message, innerException)
        {}

        public EntityException(SerializationInfo info, StreamingContext context)
            : base (info, context)
        {}
    }
}
